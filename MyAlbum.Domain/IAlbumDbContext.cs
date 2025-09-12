using System;
namespace MyAlbum.Domain
{
    public interface IAlbumDbContext : IDisposable
    {
        Task<bool> CanConnectAsync(CancellationToken ct = default);
        // 需要時可再加查詢/儲存方法或抽更細的 Repository 介面
    }
}

