using System;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;

namespace MyAlbum.Application.EmployeeAccount.implement
{
    public sealed class EmployeeAccountReadService : BaseService, IEmployeeAccountReadService
    {
        private readonly IEmployeeAccountReadRepository _repo;
        public EmployeeAccountReadService(IAlbumDbContextFactory factory, IEmployeeAccountReadRepository repo) : base(factory)
        {
            _repo = repo;
        }

        public async Task<ResponseBase<AccountDto?>> GetEmployeeAsync(GetEmployeeReq req, CancellationToken ct = default)
        {
            return await _repo.GetEmployeeAsync(req, ct);
        }
    }
}

