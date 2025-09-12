using System;
using MyAlbum.Domain;
using MyAlbum.Shared.Enums;

namespace MyAlbum.Application
{
	public class BaseService
	{
        private readonly IAlbumDbContextFactory _factory;
        protected BaseService(IAlbumDbContextFactory factory) => _factory = factory;

        protected IAlbumDbContext MainDB(ConnectionMode mode = ConnectionMode.Master) => _factory.Create(mode);
    }
}

