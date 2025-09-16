using System;
using MyAlbum.Models;
using MyAlbum.Models.Employee;

namespace MyAlbum.Application.EmployeeAccount
{
	public interface IEmployeeAccountCreateService
	{
        Task<ResponseBase<int>> CreateEmployee(CreateEmployeeReq req, CancellationToken ct = default);
    }
}

