using System;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Infrastructure.EF.Data;

namespace MyAlbum.Infrastructure.EF
{
    public sealed class EfAlbumDbContextAdapter : IAlbumDbContext
    {
        private readonly AlbumContext _ctx;

        // 讓同一個組件(Album.EF)能取回 EF 的 DbContext
        internal DbContext Db => _ctx;

        public EfAlbumDbContextAdapter(AlbumContext ctx) => _ctx = ctx;

        public Task<bool> CanConnectAsync(CancellationToken ct = default)
            => _ctx.Database.CanConnectAsync(ct);

        public void Dispose() => _ctx.Dispose();
    }
}

