using System;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;

namespace MyAlbum.Infrastructure.EF
{
    internal static class DbContextAccessors
    {
        internal static T AsDbContext<T>(this IAlbumDbContext ctx) where T : DbContext
        {
            if (ctx is EfAlbumDbContextAdapter ef) return (T)ef.Db;
            throw new InvalidOperationException("IAlbumDbContext 不是 EF 介面實作。");
        }
    }
}

