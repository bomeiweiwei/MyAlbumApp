using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.EmpRole;
using MyAlbum.Infrastructure.EF;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models;
using MyAlbum.Shared.Enums;

namespace MyAlbum.Infrastructure.Repositories.EmpRole
{
    public class EmployeeRoleReadRepository: IEmployeeRoleReadRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public EmployeeRoleReadRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<ResponseBase<List<int>>> GetEmployeeRolesAsync(int employeeId, CancellationToken ct = default)
        {
            var result = new ResponseBase<List<int>>();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<AlbumContext>();

            result.Data = await (from emp in db.Employees
                                 join empRole in db.EmployeeRoles on emp.EmployeeId equals empRole.EmployeeId
                                 where emp.EmployeeId == employeeId
                                 select empRole.RoleId).ToListAsync(ct);
            return result;
        }
    }
}
