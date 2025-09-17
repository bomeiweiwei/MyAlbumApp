using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;

namespace MyAlbum.Application.EmployeeAccount.implement
{
    public class EmployeeAccountUpdateService : BaseService, IEmployeeAccountUpdateService
    {
        private readonly IEmployeeAccountUpdateRepository _repo;
        private readonly IPasswordHasher<AccountDto> _hasher;
        public EmployeeAccountUpdateService(IAlbumDbContextFactory factory, IEmployeeAccountUpdateRepository repo, IPasswordHasher<AccountDto> hasher) : base(factory)
        {
            _repo = repo;
            _hasher = hasher;
        }
        public async Task<ResponseBase<bool>> UpdateEmployee(UpdateEmployeeReq req, CancellationToken ct = default)
        {
            if (!string.IsNullOrWhiteSpace(req.NewPassword))
            {
                // Hash 密碼
                req.NewPasswordHash = _hasher.HashPassword(null!, req.NewPassword);
                // 清掉明文，避免不小心被記錄到 log
                req.NewPassword = null;
            }
            return await _repo.UpdateEmployeeAsync(req, ct);
        }

        public async Task<ResponseBase<bool>> UpdateEmployeeActive(UpdateEmployeeActiveReq req, CancellationToken ct = default)
        {
            return await _repo.UpdateEmployeeActiveAsync(req, ct);
        }
    }
}
