using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAlbum.Models;

namespace MyAlbum.Application.RolePerm
{
    public interface IEmpRolePermissionService
    {
        /// <summary>
        /// 取得員工角色權限代碼
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ResponseBase<List<string>>> GetEmpRolePermissionCodesAsync(int empId, CancellationToken ct = default);
    }
}
