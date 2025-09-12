using System;
using Microsoft.EntityFrameworkCore;

namespace MyAlbum.Infrastructure.EF.Extensions
{
    public static class OptionsBuilderExtension
    {
        public static DbContextOptionsBuilder OptionsBuilderSetting(
            this DbContextOptionsBuilder optionsBuilder,
            string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            /// 預設走 SQL Server
            optionsBuilder.UseSqlServer(
                connectionString,
                options => options.EnableRetryOnFailure()
            );

            return optionsBuilder;
        }
    }
}

