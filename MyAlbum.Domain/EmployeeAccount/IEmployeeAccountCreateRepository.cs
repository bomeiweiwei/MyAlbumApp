using System;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;

namespace MyAlbum.Domain.EmployeeAccount
{
	public interface IEmployeeAccountCreateRepository
	{
        Task<ResponseBase<int>> CreateEmployeeWithAccountAsync(AccountCreate account, EmployeeCreate employee, CancellationToken ct = default);
    }
}

