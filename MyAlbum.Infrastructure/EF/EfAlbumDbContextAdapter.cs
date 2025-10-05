using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MyAlbum.Domain;
using MyAlbum.Infrastructure.EF.Data;

namespace MyAlbum.Infrastructure.EF
{
    sealed class EfTransaction : ITransaction
    {
        private readonly IDbContextTransaction _inner;
        public EfTransaction(IDbContextTransaction inner) => _inner = inner;
        public Task CommitAsync(CancellationToken ct = default) => _inner.CommitAsync(ct);
        public ValueTask DisposeAsync() => _inner.DisposeAsync();
    }

    public sealed class EfAlbumDbContextAdapter : IAlbumDbContext
    {
        private readonly AlbumContext _ctx;
        internal DbContext Db => _ctx;

        public EfAlbumDbContextAdapter(AlbumContext ctx) => _ctx = ctx;

        public Task<bool> CanConnectAsync(CancellationToken ct = default)
             => _ctx.Database.CanConnectAsync(ct);

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _ctx.SaveChangesAsync(ct);

        public async Task<ITransaction> BeginTransactionAsync(CancellationToken ct = default)
            => new EfTransaction(await _ctx.Database.BeginTransactionAsync(ct));

        public void Dispose() => _ctx.Dispose();
    }
}

