using System;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Domain.Perm;
using MyAlbum.Infrastructure.EF;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models;
using MyAlbum.Models.Perm;
using MyAlbum.Shared.Enums;

namespace MyAlbum.Infrastructure.Repositories.Perm
{
    public sealed class PermissionReadRepository : IPermissionReadRepository
    {
        private readonly IAlbumDbContextFactory _factory;
        public PermissionReadRepository(IAlbumDbContextFactory factory) => _factory = factory;

        public async Task<ResponseBase<List<PermissionDto>>> GetPermissionListAsync(PageRequestBase<PermissionListReq> req, CancellationToken ct = default)
        {
            var result = new ResponseBase<List<PermissionDto>>();
            using var ctx = _factory.Create(ConnectionMode.Slave);
            var db = ctx.AsDbContext<AlbumContext>();

            var query = from p in db.Permissions
                        select new PermissionDto()
                        {
                            PermissionId = p.PermissionId,
                            Code = p.Code,
                            Description = p.Description
                        };
            if (!string.IsNullOrWhiteSpace(req.Data.Code))
            {
                query = query.Where(m => m.Code.Contains(req.Data.Code));
            }
            result.Count = await query.CountAsync();
            result.Data = await query.ToListAsync(ct);
            return result;
        }
    }
}

