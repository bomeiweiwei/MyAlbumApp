using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAlbum.Domain;
using MyAlbum.Domain.Member;
using MyAlbum.Infrastructure.EF;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Member;

namespace MyAlbum.Infrastructure.Repositories.Member
{
    public sealed class MemberWriteRepository : IMemberWriteRepository
    {
        public async Task<int> CreateAsync(IAlbumDbContext ctx, MemberCreate model, CancellationToken ct = default)
        {
            var db = ctx.AsDbContext<AlbumContext>();
            var entity = new EF.Models.Member
            {
                AccountId = model.AccountId,
                DisplayName = model.DisplayName,
                AvatarUrl = model.AvatarUrl,
                Bio = model.Bio,
                CreatedDate = model.CreatedDate,
                CreatedBy = model.CreatedBy
            };
            await db.Members.AddAsync(entity, ct);
            return entity.MemberId;
        }
    }
}
