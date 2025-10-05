using System;
using Microsoft.AspNetCore.Identity;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Shared.Abstractions;
using MyAlbum.Shared.Enums;

namespace MyAlbum.Application.EmployeeAccount.implement
{
    public class EmployeeAccountCreateService : BaseService, IEmployeeAccountCreateService
    {
        private readonly IEmployeeAccountCreateRepository _repo;
        private readonly IPasswordHasher<AccountDto> _hasher;
        private readonly IClock _clock;
        private readonly IGuidProvider _guid;
        public EmployeeAccountCreateService(
           IAlbumDbContextFactory factory,
           IEmployeeAccountCreateRepository repo,
           IPasswordHasher<AccountDto> hasher,
           IClock clock,
           IGuidProvider guid) : base(factory)
        {
            _repo = repo;
            _hasher = hasher;
            _clock = clock;
            _guid = guid;
        }

        public async Task<ResponseBase<int>> CreateEmployeeWithAccount(CreateEmployeeReq req, CancellationToken ct = default)
        {
            var login = req.LoginName.Trim();
            var email = req.Email.Trim();
            var normalizedLogin = login.ToUpperInvariant();
            var normalizedEmail = email.ToUpperInvariant();
            var passwordHash = _hasher.HashPassword(null!, req.Password);
            var now = _clock.UtcNow;
            var accountId = _guid.NewGuid();
            var securityStamp = _guid.NewGuid();
            var account = new AccountCreate
            {
                AccountId = accountId,
                LoginName = login,
                NormalizedLoginName = normalizedLogin,
                Email = email,
                NormalizedEmail = normalizedEmail,
                EmailConfirmed = true,
                PasswordHash = passwordHash,
                SecurityStamp = securityStamp,
                UserType = (byte)LoginUserType.Employee,
                IsActive = true,
                CreatedDate = now,
                CreatedBy = req.OperatorId
            };

            var employee = new EmployeeCreate
            {
                AccountId = accountId,
                FullName = req.FullName.Trim(),
                Title = req.Title,
                HireDate = req.HireDate,
                IsActive = true,
                CreatedDate = now,
                CreatedBy = req.OperatorId
            };
            return await _repo.CreateEmployeeWithAccountAsync(account, employee, ct);
        }
    }
}

