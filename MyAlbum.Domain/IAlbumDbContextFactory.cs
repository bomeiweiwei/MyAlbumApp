using System;
using MyAlbum.Shared.Enums;

namespace MyAlbum.Domain
{
	public interface IAlbumDbContextFactory
	{
        IAlbumDbContext Create(ConnectionMode mode = ConnectionMode.Master);
    }
}

