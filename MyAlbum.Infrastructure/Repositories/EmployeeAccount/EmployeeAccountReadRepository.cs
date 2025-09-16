using System;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Infrastructure.EF;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Infrastructure.EF.Extensions;
using MyAlbum.Infrastructure.EF.Models;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;

namespace MyAlbum.Infrastructure.Repositories.EmployeeAccount
{
    public sealed class EmployeeAccountReadRepository : IEmployeeAccountReadRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public EmployeeAccountReadRepository(IAlbumDbContextFactory factory) => _factory = factory;
        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ResponseBase<AccountDto?>> GetEmployeeAsync(GetEmployeeReq req, CancellationToken ct = default)
        {
            var result = new ResponseBase<AccountDto?>();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<AlbumContext>();

            var query = from emp in db.Employees.AsNoTracking()
                        join account in db.Accounts.AsNoTracking() on emp.AccountId equals account.AccountId
                        where account.UserType == 1
                        select new AccountDto()
                        {
                            EmployeeId = emp.EmployeeId,
                            AccountId = account.AccountId,
                            LoginName = account.LoginName,
                            IsActive = account.IsActive,
                            UserType = LoginUserType.Employee.GetDescription(),
                            FullName = emp.FullName,
                            PasswordHash = account.PasswordHash,
                            Title = emp.Title ?? "",
                            HireDate = emp.HireDate,
                            Email = account.Email ?? ""
                        };
            if (req.EmployeeId > 0)
                query = query.Where(m => m.EmployeeId == req.EmployeeId);
            if (!string.IsNullOrWhiteSpace(req.LoginName))
                query = query.Where(m => m.LoginName == req.LoginName);

            result.Data = await query.FirstOrDefaultAsync(ct);
            return result;
        }
        /// <summary>
        /// 取得員工列表資料
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ResponseBase<List<EmployeeItem>>> GetEmployeeListAsync(PageRequestBase<EmployeeListReq> req, CancellationToken ct = default)
        {
            var result = new ResponseBase<List<EmployeeItem>>()
            {
                Data = new List<EmployeeItem>()
            };

            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<AlbumContext>();

            var query = from emp in db.Employees.AsNoTracking()
                        join account in db.Accounts.AsNoTracking() on emp.AccountId equals account.AccountId
                        select new EmployeeItem()
                        {
                            EmployeeId = emp.EmployeeId,
                            FullName = emp.FullName,
                            Title = emp.Title,
                            HireDate = emp.HireDate,
                            IsActive = emp.IsActive
                        };
            if (!string.IsNullOrWhiteSpace(req.Data.FullName))
            {
                query = query.Where(m => m.FullName.Contains(req.Data.FullName));
            }

            result.Count = await query.CountAsync();
            result.Data = await query.Pagination(req.PageIndex, req.PageSize).ToListAsync(ct);
            return result;
        }
    }
}

