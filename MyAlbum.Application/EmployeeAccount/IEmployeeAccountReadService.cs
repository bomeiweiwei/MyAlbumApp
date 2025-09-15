using System;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;

namespace MyAlbum.Application.EmployeeAccount
{
	public interface IEmployeeAccountReadService
	{
        Task<ResponseBase<AccountDto?>> GetEmployeeAsync(GetEmployeeReq req, CancellationToken ct = default);

        Task<ResponseBase<List<EmployeeItem>>> GetEmployeeListAsync(PageRequestBase<EmployeeListReq> req, CancellationToken ct = default);

        Task<ResponseBase<EmployeeDto>> GetEmployeeDataByIdAsync(GetEmployeeReq req, CancellationToken ct = default);
    }
}

