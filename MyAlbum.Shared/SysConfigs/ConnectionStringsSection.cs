using System;
using Microsoft.Extensions.Configuration;

namespace MyAlbum.Shared.SysConfigs
{
	public class ConnectionStringsSection
	{
        public string Master { get; }
        public string Slave { get; }

        public ConnectionStringsSection(IConfiguration config)
        {
            var sec = config.GetRequiredSection("ConnectionStrings");
            Master = sec["MasterConnection"]
                     ?? throw new InvalidOperationException("Missing ConnectionStrings:MasterConnection");
            Slave = sec["SlaveConnection"]
                     ?? throw new InvalidOperationException("Missing ConnectionStrings:SlaveConnection");
        }
    }
}

