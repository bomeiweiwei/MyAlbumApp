using System;
using MyAlbum.Domain;
using MyAlbum.Shared.Enums;

namespace MyAlbum.Application.Test.implement
{
    public class TestService : BaseService, ITestService
    {
        public TestService(IAlbumDbContextFactory factory) : base(factory) { }

        public async Task<bool> GetConnectResult()
        {
            using var ctx = MainDB(ConnectionMode.Slave); // or Master
            return await ctx.CanConnectAsync();
        }
    }
}

