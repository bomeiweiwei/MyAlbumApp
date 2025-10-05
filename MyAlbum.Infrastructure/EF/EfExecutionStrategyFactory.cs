using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Infrastructure.EF.Data;

namespace MyAlbum.Infrastructure.EF
{
    public sealed class EfExecutionStrategyFactory : IExecutionStrategyFactory
    {
        public IExecutionStrategy Create(IAlbumDbContext ctx)
        {
            var db = (ctx as EfAlbumDbContextAdapter)?.Db as AlbumContext
                     ?? throw new InvalidOperationException("Not EF context");

            var efStrategy = db.Database.CreateExecutionStrategy();
            return new StrategyWrapper(efStrategy);
        }

        private sealed class StrategyWrapper : IExecutionStrategy
        {
            private readonly Microsoft.EntityFrameworkCore.Storage.IExecutionStrategy _inner;
            public StrategyWrapper(Microsoft.EntityFrameworkCore.Storage.IExecutionStrategy inner) => _inner = inner;
            public Task ExecuteAsync(Func<Task> operation, CancellationToken ct = default)
                => _inner.ExecuteAsync(operation);
        }
    }
}
