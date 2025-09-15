using System;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;

namespace MyAlbum.Domain.EmployeeAccount
{
	public interface IEmployeeAccountReadRepository
	{
        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ResponseBase<AccountDto?>> GetEmployeeAsync(GetEmployeeReq req, CancellationToken ct = default);
    }
}

