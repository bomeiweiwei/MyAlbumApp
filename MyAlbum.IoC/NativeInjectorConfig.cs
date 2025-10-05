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

            // 固定線
            services.AddScoped<IAlbumDbContextFactory, AlbumDbContextFactory>();
            // 重試策略
            services.AddScoped<IExecutionStrategyFactory, EfExecutionStrategyFactory>();

            services.AddSingleton<IClock, SystemClock>();
            services.AddSingleton<IGuidProvider, GuidProvider>();

            // 慣例掃描
            var servicesAsm = typeof(BaseService).Assembly;
            var efAsm = typeof(AlbumDbContextFactory).Assembly;
            services.RegisterByConvention(servicesAsm, efAsm);

        }
    }
}

