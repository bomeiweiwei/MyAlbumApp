using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAlbum.Models.Account;

namespace MyAlbum.Domain.Account
{
    public interface IAccountWriteRepository
    {
        Task<Guid> CreateAsync(IAlbumDbContext ctx, AccountCreate model, CancellationToken ct = default);
    }
}