using System;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Domain;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Infrastructure.EF.Extensions;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.SysConfigs;

namespace MyAlbum.Infrastructure.EF
{
    public sealed class AlbumDbContextFactory : IAlbumDbContextFactory
    {
        public IAlbumDbContext Create(ConnectionMode mode = ConnectionMode.Master)
        {
            var options = new DbContextOptionsBuilder<AlbumContext>();

            var cs = mode == ConnectionMode.Master
                ? ConfigManager.ConnectionStrings.Master
                : ConfigManager.ConnectionStrings.Slave;

            options.OptionsBuilderSetting(cs); // 你既有的擴充/設定

            var ctx = new AlbumContext(options.Options);
            return new EfAlbumDbContextAdapter(ctx);
        }
    }
}

