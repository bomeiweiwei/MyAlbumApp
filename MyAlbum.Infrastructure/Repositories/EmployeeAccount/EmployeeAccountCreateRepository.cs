using System;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Infrastructure.EF;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Infrastructure.EF.Models;
using MyAlbum.Models;
using MyAlbum.Models.Employee;
using MyAlbum.Shared.Enums;

namespace MyAlbum.Infrastructure.Repositories.EmployeeAccount
{
    public sealed class EmployeeAccountCreateRepository : IEmployeeAccountCreateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public EmployeeAccountCreateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<ResponseBase<int>> CreateEmployeeAsync(CreateEmployeeReq req, CancellationToken ct = default)
        {
            var result = new ResponseBase<int>();

            // 寫入用 Master 連線
            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<AlbumContext>();

            // 暫時性錯誤的重試策略（例如連線中斷）
            var strategy = db.Database.CreateExecutionStrategy();

            try
            {
                await strategy.ExecuteAsync(async () =>
                {
                    await using var tx = await db.Database.BeginTransactionAsync(ct);
                    try
                    {
                        // 1) 基本的唯一性檢查（也建議在 DB 設 UNIQUE 索引）
                        var normalizedLogin = req.LoginName.Trim().ToUpperInvariant();
                        var normalizedEmail = req.Email.Trim().ToUpperInvariant();

                        bool loginExists = await db.Accounts
                            .AsNoTracking()
                            .AnyAsync(a => a.NormalizedLoginName == normalizedLogin, ct);

                        if (loginExists)
                            throw new InvalidOperationException("LoginName 已存在。");

                        bool emailExists = await db.Accounts
                            .AsNoTracking()
                            .AnyAsync(a => a.NormalizedEmail == normalizedEmail, ct);

                        if (emailExists)
                            throw new InvalidOperationException("Email 已存在。");

                        var now = DateTime.UtcNow;

                        // 2) 先建 Account
                        var account = new Account
                        {
                            AccountId = Guid.NewGuid(),
                            LoginName = req.LoginName.Trim(),
                            NormalizedLoginName = normalizedLogin,
                            Email = req.Email.Trim(),
                            NormalizedEmail = normalizedEmail,
                            EmailConfirmed = false,
                            PasswordHash = req.PasswordHash,
                            SecurityStamp = Guid.NewGuid(),
                            UserType = (byte)LoginUserType.Employee,
                            IsActive = true,
                            CreatedDate = now,
                            CreatedBy = req.OperatorId
                        };
                        await db.Accounts.AddAsync(account, ct);

                        // 3) 再建 Employee（外鍵指向 Account）
                        var employee = new Employee
                        {
                            // EmployeeId 是 IDENTITY 讓 DB 產生
                            AccountId = account.AccountId,
                            FullName = req.FullName.Trim(),
                            Title = req.Title,
                            HireDate = req.HireDate,
                            IsActive = true,
                            CreatedDate = now,
                            CreatedBy = req.OperatorId
                        };
                        await db.Employees.AddAsync(employee, ct);

                        // 4) 一次 SaveChanges
                        await db.SaveChangesAsync(ct);

                        // 5) Commit
                        await tx.CommitAsync(ct);

                        result.Data = employee.EmployeeId;
                        result.StatusCode = (long)ReturnCode.Succeeded;
                    }
                    catch
                    {
                        await tx.RollbackAsync(ct);
                        throw;
                    }
                });
            }
            catch (InvalidOperationException ex) // 自訂的商業錯誤
            {
                result.Data = 0;
                result.StatusCode = (long)ReturnCode.ExceptionError;
                result.Message = ex.Message;
            }
            catch (DbUpdateException ex) // 例如 UNIQUE 衝突
            {
                result.Data = 0;
                result.StatusCode = (long)ReturnCode.ExceptionError;
                result.Message = "資料庫更新失敗：" + ex.GetBaseException().Message;
            }
            catch (Exception ex)
            {
                result.Data = 0;
                result.StatusCode = (long)ReturnCode.ExceptionError;
                result.Message = "發生未預期錯誤：" + ex.Message;
            }

            return result;
        }
    }
}

