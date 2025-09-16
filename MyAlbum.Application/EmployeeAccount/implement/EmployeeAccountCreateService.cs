using System;
using Microsoft.AspNetCore.Identity;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;

namespace MyAlbum.Application.EmployeeAccount.implement
{
    public class EmployeeAccountCreateService : BaseService, IEmployeeAccountCreateService
    {
        private readonly IEmployeeAccountCreateRepository _repo;
        private readonly IPasswordHasher<AccountDto> _hasher;
        public EmployeeAccountCreateService(IAlbumDbContextFactory factory, IEmployeeAccountCreateRepository repo, IPasswordHasher<AccountDto> hasher) : base(factory)
        {
            _repo = repo;
            _hasher = hasher;
        }

        public async Task<ResponseBase<int>> CreateEmployee(CreateEmployeeReq req, CancellationToken ct = default)
        {
            // Hash 密碼
            req.PasswordHash = _hasher.HashPassword(null!, req.Password);
            // 清掉明文，避免不小心被記錄到 log
            req.Password = null;
            return await _repo.CreateEmployeeAsync(req, ct);
        }
    }
}

