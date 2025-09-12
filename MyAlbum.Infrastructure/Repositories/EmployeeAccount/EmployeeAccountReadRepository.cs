using System;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Infrastructure.EF;
using MyAlbum.Infrastructure.EF.Data;
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
        public async Task<AccountDto?> GetEmployeeAsync(GetEmployeeReq req, CancellationToken ct = default)
        {
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<AlbumContext>();

            var query = await (from emp in db.Employees.AsNoTracking()
                               join account in db.Accounts.AsNoTracking() on emp.AccountId equals account.AccountId
                               where account.LoginName == req.LoginName && account.UserType == 1
                               select new AccountDto()
                               {
                                   AccountId = account.AccountId,
                                   LoginName = account.LoginName,
                                   IsActive = account.IsActive,
                                   UserType = LoginUserType.Employee.GetDescription(),
                                   FullName = emp.FullName
                               }).FirstOrDefaultAsync(ct);
            return query;
        }
    }
}

