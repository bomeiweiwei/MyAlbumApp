using System;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Infrastructure.EF;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Infrastructure.EF.Models;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;

namespace MyAlbum.Infrastructure.Repositories.EmployeeAccount
{
    public sealed class EmployeeAccountCreateRepository : IEmployeeAccountCreateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public EmployeeAccountCreateRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<ResponseBase<int>> CreateEmployeeWithAccountAsync(AccountCreate accountModel, EmployeeCreate employeeModel, CancellationToken ct = default)
        {
            var result = new ResponseBase<int>();
            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<AlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            try
            {
                var normalizedLogin = accountModel.NormalizedLoginName;
                var normalizedEmail = accountModel.NormalizedEmail;

                var exists = await db.Accounts.AsNoTracking()
                    .Where(a => a.NormalizedLoginName == normalizedLogin || a.NormalizedEmail == normalizedEmail)
                    .Select(a => new { a.NormalizedLoginName, a.NormalizedEmail })
                    .FirstOrDefaultAsync(ct);

                if (exists != null)
                {
                    if (exists.NormalizedLoginName == normalizedLogin)
                        throw new InvalidOperationException("LoginName 已存在。");
                    if (exists.NormalizedEmail == normalizedEmail)
                        throw new InvalidOperationException("Email 已存在。");
                }

                await strategy.ExecuteAsync(async () =>
                {
                    await using var tx = await db.Database.BeginTransactionAsync(ct);
                    try
                    {
                        var account = new EF.Models.Account
                        {
                            AccountId = accountModel.AccountId,
                            LoginName = accountModel.LoginName,
                            NormalizedLoginName = accountModel.NormalizedLoginName,
                            Email = accountModel.Email,
                            NormalizedEmail = accountModel.NormalizedEmail,
                            EmailConfirmed = accountModel.EmailConfirmed,
                            PasswordHash = accountModel.PasswordHash,
                            SecurityStamp = accountModel.SecurityStamp,
                            UserType = accountModel.UserType,
                            IsActive = accountModel.IsActive,
                            CreatedDate = accountModel.CreatedDate,
                            CreatedBy = accountModel.CreatedBy
                        };
                        await db.Accounts.AddAsync(account, ct);

                        var employee = new Employee
                        {
                            AccountId = employeeModel.AccountId,
                            FullName = employeeModel.FullName,
                            Title = employeeModel.Title,
                            HireDate = employeeModel.HireDate,
                            IsActive = employeeModel.IsActive,
                            CreatedDate = employeeModel.CreatedDate,
                            CreatedBy = employeeModel.CreatedBy
                        };
                        await db.Employees.AddAsync(employee, ct);

                        await db.SaveChangesAsync(ct);

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
                result.StatusCode = (long)ReturnCode.BusinessError;
                result.Message = ex.Message;
            }
            catch (DbUpdateException ex) // 例如 UNIQUE 衝突
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

