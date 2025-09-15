using System;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Models.RolePerm;

namespace MyAlbum.Domain.RolePerm
{
	public interface IRolePermissionReadRepository
	{
        Task<ResponseBase<List<string>>> GetRolePermissionCodesAsync(List<int> roleIds, CancellationToken ct = default);
    }
}

