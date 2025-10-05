using System;
using MyAlbum.Models;
using MyAlbum.Models.Employee;

namespace MyAlbum.Application.EmployeeAccount
{
	public interface IEmployeeAccountCreateService
	{
        Task<ResponseBase<int>> CreateEmployeeWithAccount(CreateEmployeeReq req, CancellationToken ct = default);
    }
}

