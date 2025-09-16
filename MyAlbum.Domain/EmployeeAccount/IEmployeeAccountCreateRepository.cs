using System;
using MyAlbum.Models;
using MyAlbum.Models.Employee;

namespace MyAlbum.Domain.EmployeeAccount
{
	public interface IEmployeeAccountCreateRepository
	{
        Task<ResponseBase<int>> CreateEmployeeAsync(CreateEmployeeReq req, CancellationToken ct = default);
    }
}

