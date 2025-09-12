using System;
using Microsoft.Extensions.Configuration;

namespace MyAlbum.Shared.SysConfigs
{
	public class ConfigManager
	{
        public static ConnectionStringsSection ConnectionStrings { get; private set; } = default!;

        public static void Initial(IConfiguration configuration)
        {
            ConnectionStrings = new ConnectionStringsSection(configuration);
        }
    }
}

