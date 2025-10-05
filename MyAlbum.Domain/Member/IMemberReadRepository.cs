using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAlbum.Models.Member;

namespace MyAlbum.Domain.Member
{
    public interface IMemberReadRepository
    {
        Task<MemberDto?> GetAsync(int memberId, CancellationToken ct = default);
        Task<List<MemberItem>> QueryAsync(MemberQuery req, CancellationToken ct = default);
    }
}
