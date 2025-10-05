using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Account;
using MyAlbum.Domain.Member;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Models.Member;
using MyAlbum.Shared.Abstractions;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;

namespace MyAlbum.Application.MemberAccount.implement
{
    public class MemberAccountWriteService : BaseService, IMemberAccountWriteService
    {
        private readonly IMemberWriteRepository _memRepo;
        private readonly IAccountWriteRepository _accRepo;
        private readonly IPasswordHasher<AccountDto> _hasher;
        private readonly IExecutionStrategyFactory _strategyFactory;
        private readonly IClock _clock;
        private readonly IGuidProvider _guid;

        public MemberAccountWriteService(
            IAlbumDbContextFactory factory,
            IMemberWriteRepository memRepo,
            IAccountWriteRepository accRepo,
            IPasswordHasher<AccountDto> hasher,
            IExecutionStrategyFactory strategyFactory,
            IClock clock,
            IGuidProvider guid) : base(factory)
        {
            _memRepo = memRepo;
            _accRepo = accRepo;
            _hasher = hasher;
            _strategyFactory = strategyFactory;
            _clock = clock;
            _guid = guid;
        }

        public async Task<ResponseBase<int>> CreateMemberWithAccountAsync(CreateMemberReq req, CancellationToken ct = default)
        {
            var result = new ResponseBase<int>();

            var normalizedLogin = req.LoginName.Trim().ToUpperInvariant();
            var normalizedEmail = req.Email.Trim().ToUpperInvariant();
            var passwordHash = _hasher.HashPassword(null!, req.Password ?? "");
            req.Password = null;

            var now = _clock.UtcNow;
            var accountId = _guid.NewGuid();
            var memberId = 0;

            using var ctx = MainDB(ConnectionMode.Master);
            var strategy = _strategyFactory.Create(ctx);

            try
            {
                await strategy.ExecuteAsync(async () =>
                {
                    await using var tx = await ctx.BeginTransactionAsync(ct);

                    var accId = await _accRepo.CreateAsync(ctx, new AccountCreate
                    {
                        AccountId = accountId,
                        LoginName = req.LoginName.Trim(),
                        NormalizedLoginName = normalizedLogin,
                        Email = req.Email.Trim(),
                        NormalizedEmail = normalizedEmail,
                        PasswordHash = passwordHash,
                        UserType = (byte)LoginUserType.Member,
                        CreatedDate = now,
                        CreatedBy = req.OperatorId
                    }, ct);

                    memberId = await _memRepo.CreateAsync(ctx, new MemberCreate
                    {
                        AccountId = accId,
                        DisplayName = req.LoginName.Trim(),
                        CreatedDate = now,
                        CreatedBy = req.OperatorId
                    }, ct);

                    await ctx.SaveChangesAsync(ct);
                    await tx.CommitAsync(ct);
                }, ct);

                result.Data = memberId;
            }
            catch (InvalidOperationException ex)
            {
                result.StatusCode = (long)ReturnCode.BusinessError;
                result.Message = ReturnCode.BusinessError.GetDescription();
            }
            catch (DbUpdateException ex) // 如果你選擇讓 Repo bubble EF 例外上來
            {
                result.StatusCode = (long)ReturnCode.DbUpdateError;
                result.Message = ReturnCode.DbUpdateError.GetDescription();
            }
            catch (Exception ex)
            {
                result.StatusCode = (long)ReturnCode.ExceptionError;
                result.Message = ReturnCode.ExceptionError.GetDescription();
            }
            return result;
        }
    }
}
