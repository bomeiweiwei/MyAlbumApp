using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Infrastructure.EF;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models;
using MyAlbum.Models.Employee;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;

namespace MyAlbum.Infrastructure.Repositories.EmployeeAccount
{
    public class EmployeeAccountUpdateRepository : IEmployeeAccountUpdateRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public EmployeeAccountUpdateRepository(IAlbumDbContextFactory factory) => _factory = factory;
        public async Task<ResponseBase<bool>> UpdateEmployeeAsync(UpdateEmployeeReq req, CancellationToken ct = default)
        {
            var result = new ResponseBase<bool>();

            using var ctx = _factory.Create(ConnectionMode.Master);
            var db = ctx.AsDbContext<AlbumContext>();
            var strategy = db.Database.CreateExecutionStrategy();

            try
            {
                // 先抓目標員工與其 Account
                var employee = await db.Employees
                    .FirstOrDefaultAsync(m => m.EmployeeId == req.EmployeeId, ct);
                if (employee == null)
                    throw new InvalidOperationException("員工不存在。");

                var account = await (from emp in db.Employees
                                     join acc in db.Accounts on emp.AccountId equals acc.AccountId
                                     where emp.EmployeeId == employee.EmployeeId
                                           && acc.UserType == (byte)LoginUserType.Employee
                                     select acc).FirstOrDefaultAsync(ct);

                if (account == null)
                    throw new InvalidOperationException("帳號不存在。");

                var now = DateTime.UtcNow;

                // 準備變更
                var incomingEmail = string.IsNullOrWhiteSpace(req.Email) ? null : req.Email.Trim();
                var emailChanged = !string.Equals(account.Email, incomingEmail, StringComparison.OrdinalIgnoreCase);

                if (emailChanged && incomingEmail != null)
                {
                    var normalizedEmail = incomingEmail.ToUpperInvariant();
                    // 查重只在 email 變更時
                    bool emailExists = await db.Accounts
                        .AsNoTracking()
                        .AnyAsync(a => a.AccountId != account.AccountId &&
                                       a.NormalizedEmail == normalizedEmail, ct);
                    if (emailExists)
                        throw new InvalidOperationException("Email 已存在，無法修改。");
                }

                var passwordChanged = !string.IsNullOrWhiteSpace(req.NewPasswordHash);

                await strategy.ExecuteAsync(async () =>
                {
                    await using var tx = await db.Database.BeginTransactionAsync(ct);
                    try
                    {
                        // ---- Account ----
                        // 只改 Email，本來的 NormalizedEmail 讓 DB 自己算（PERSISTED 計算欄位）
                        account.Email = incomingEmail;      // <— 不要設定 NormalizedEmail
                        if (passwordChanged)
                            account.PasswordHash = req.NewPasswordHash;

                        if (emailChanged || passwordChanged)
                            account.SecurityStamp = Guid.NewGuid(); // 安全性相關變更才換

                        account.UpdatedDate = now;
                        account.UpdatedBy = req.OperatorId;

                        // ---- Employee ----
                        employee.FullName = req.FullName?.Trim() ?? employee.FullName;  // FullName 若必填，可先驗證
                        employee.Title = string.IsNullOrWhiteSpace(req.Title) ? null : req.Title.Trim();

                        // 若允許清空 HireDate，將 UpdateEmployeeReq.HireDate 改成 DateOnly?
                        employee.HireDate = req.HireDate; // 若改成 nullable 記得處理

                        employee.UpdatedDate = now;
                        employee.UpdatedBy = req.OperatorId;

                        // 一次 SaveChanges
                        await db.SaveChangesAsync(ct);
                        await tx.CommitAsync(ct);

                        result.Data = true;
                        result.StatusCode = (long)ReturnCode.Succeeded;
                    }
                    catch
                    {
                        await tx.RollbackAsync(ct);
                        throw;
                    }
                });
            }
            catch (InvalidOperationException ex)
            {
                result.Data = false;
                result.StatusCode = (long)ReturnCode.ExceptionError;
                result.Message = ex.Message;
            }
            catch (DbUpdateException ex)
            {
                result.Data = false;
                result.StatusCode = (long)ReturnCode.ExceptionError;
                result.Message = "資料庫更新失敗：" + ex.GetBaseException().Message;
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.StatusCode = (long)ReturnCode.ExceptionError;
                result.Message = "發生未預期錯誤：" + ex.Message;
            }

            return result;
        }
    }
}
