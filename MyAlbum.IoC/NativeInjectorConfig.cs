using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAlbum.Application;
using MyAlbum.Domain;
using MyAlbum.Infrastructure.EF;
using MyAlbum.Models.Account;
using MyAlbum.Shared.Abstractions;
using MyAlbum.Shared.Implementations;

namespace MyAlbum.IoC
{
    public static class NativeInjectorConfig
    {
        public static void RegisterService(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IPasswordHasher<AccountDto>, PasswordHasher<AccountDto>>();
            services.AddSingleton<IClock, SystemClock>();
            services.AddSingleton<IGuidProvider, GuidProvider>();

            // 固定線
            services.AddScoped<IAlbumDbContextFactory, AlbumDbContextFactory>();

            // 慣例掃描（一次設定好就不動）
            var servicesAsm = typeof(BaseService).Assembly;
            var efAsm = typeof(AlbumDbContextFactory).Assembly;
            services.RegisterByConvention(servicesAsm, efAsm);

        }
    }
}

