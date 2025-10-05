using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MyAlbum.Domain;
using MyAlbum.Domain.Account;
using MyAlbum.Infrastructure.EF;
using MyAlbum.Infrastructure.EF.Data;
using MyAlbum.Models.Account;

namespace MyAlbum.Infrastructure.Repositories.Account
{
    public sealed class AccountWriteRepository : IAccountWriteRepository
    {
        public async Task<Guid> CreateAsync(IAlbumDbContext ctx, AccountCreate model, CancellationToken ct = default)
        {
            var db = ((EfAlbumDbContextAdapter)ctx).Db as AlbumContext ?? throw new InvalidOperationException();
            if (await db.Accounts.AsNoTracking().AnyAsync(a => a.NormalizedLoginName == model.NormalizedLoginName, ct))
                throw new InvalidOperationException("LoginName 已存在。");
            if (await db.Accounts.AsNoTracking().AnyAsync(a => a.NormalizedEmail == model.NormalizedEmail, ct))
                throw new InvalidOperationException("Email 已存在。");

            var entity = new EF.Models.Account
            {
                AccountId = model.AccountId,
                LoginName = model.LoginName,
                NormalizedLoginName = model.NormalizedLoginName,
                Email = model.Email,
                NormalizedEmail = model.NormalizedEmail,
                EmailConfirmed = model.EmailConfirmed,
                PasswordHash = model.PasswordHash,
                SecurityStamp = model.SecurityStamp,
                UserType = model.UserType,
                IsActive = model.IsActive,
                CreatedDate = model.CreatedDate,
                CreatedBy = model.CreatedBy
            };
            await db.Accounts.AddAsync(entity, ct);
            return entity.AccountId;
        }
    }
}
