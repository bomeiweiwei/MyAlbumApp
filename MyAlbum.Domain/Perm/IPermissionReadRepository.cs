using System;
using MyAlbum.Models;
using MyAlbum.Models.Employee;
using MyAlbum.Models.Perm;

namespace MyAlbum.Domain.Perm
{
	public interface IPermissionReadRepository
	{
        Task<ResponseBase<List<PermissionDto>>> GetPermissionListAsync(PageRequestBase<PermissionListReq> req, CancellationToken ct = default);
    }
}

