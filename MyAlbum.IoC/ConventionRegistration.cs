using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MyAlbum.IoC
{
    public static class ConventionRegistration
    {
        public static void RegisterByConvention(this IServiceCollection services, Assembly servicesAsm, Assembly efAsm, ILogger? logger = null)
        {
            // Services：ClassName 以 Service 結尾，綁 I{ClassName}
            AutoBind(services, servicesAsm, nameEndsWith: "Service", logger);

            // Repositories：ClassName 以 Repository 結尾，綁 I{ClassName}
            AutoBind(services, efAsm, nameEndsWith: "Repository", logger);
        }

        private static void AutoBind(IServiceCollection services, Assembly asm, string nameEndsWith, ILogger? logger)
        {
            var impls = asm.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic && t.Name.EndsWith(nameEndsWith));

            foreach (var impl in impls)
            {
                var iface = impl.GetInterfaces().FirstOrDefault(i => i.Name == $"I{impl.Name}");
                if (iface == null)
                {
                    logger?.LogWarning("Skip auto registration: {Impl} (no interface I{ImplName})", impl.FullName, impl.Name);
                    continue; // 跳過，不拋例外，避免啟動被命名風格拖垮
                }

                services.AddScoped(iface, impl);
            }
        }
    }
}

