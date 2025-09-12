using System;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;

namespace MyAlbum.Application.EmployeeAccount
{
	public interface IEmployeeAccountReadService
	{
        Task<AccountDto?> GetEmployeeAsync(GetEmployeeReq req, CancellationToken ct = default);
    }
}

