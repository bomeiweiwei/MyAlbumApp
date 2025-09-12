using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAlbum.Application;
using MyAlbum.Domain;
using MyAlbum.Infrastructure.EF;

namespace MyAlbum.IoC
{
    public static class NativeInjectorConfig
    {
        public static void RegisterService(this IServiceCollection services, IConfiguration config)
        {
            // 固定線
            services.AddScoped<IAlbumDbContextFactory, AlbumDbContextFactory>();

            // 慣例掃描（一次設定好就不動）
            var servicesAsm = typeof(BaseService).Assembly;
            var efAsm = typeof(AlbumDbContextFactory).Assembly;
            services.RegisterByConvention(servicesAsm, efAsm);

        }
    }
}

