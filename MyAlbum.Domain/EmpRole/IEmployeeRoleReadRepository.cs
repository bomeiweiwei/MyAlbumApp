using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAlbum.Models;

namespace MyAlbum.Domain.EmpRole
{
    public interface IEmployeeRoleReadRepository
    {
        /// <summary>
        /// 取得員工角色
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ResponseBase<List<int>>> GetEmployeeRolesAsync(int employeeId, CancellationToken ct = default);
    }
}
