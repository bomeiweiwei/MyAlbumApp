using System;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Models.RolePerm;

namespace MyAlbum.Domain.RolePerm
{
	public interface IRolePermissionReadRepository
	{
        /// <summary>
        /// 取得角色權限代碼
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ResponseBase<List<string>>> GetRolePermissionCodesAsync(List<int> roleIds, CancellationToken ct = default);
    }
}

