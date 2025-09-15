using System;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.RolePerm;
using MyAlbum.Infrastructure.EF;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models;
using MyAlbum.Shared.Enums;

namespace MyAlbum.Infrastructure.Repositories.RolePerm
{
	public sealed class RolePermissionReadRepository : IRolePermissionReadRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public RolePermissionReadRepository(IAlbumDbContextFactory factory) => _factory = factory;
        /// <summary>
        /// 取得角色權限代碼
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ResponseBase<List<string>>> GetRolePermissionCodesAsync(List<int> roleIds, CancellationToken ct = default)
        {
            var result = new ResponseBase<List<string>>();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<AlbumContext>();

            result.Data = await (from role in db.Roles
                                 join rolePerm in db.RolePermissions on role.RoleId equals rolePerm.RoleId
                                 join p in db.Permissions on rolePerm.PermissionId equals p.PermissionId
                                 where roleIds.Contains(role.RoleId)
                                 select p.Code).ToListAsync(ct);
            return result;
        }
    }
}

