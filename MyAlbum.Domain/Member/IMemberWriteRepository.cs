using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAlbum.Models.Member;

namespace MyAlbum.Domain.Member
{
    public interface IMemberWriteRepository
    {
        Task<int> CreateAsync(IAlbumDbContext ctx, MemberCreate model, CancellationToken ct = default);
    }
}
