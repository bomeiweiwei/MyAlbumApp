using System;
using Microsoft.AspNetCore.Identity;
using MyAlbum.Application.EmployeeAccount;
using MyAlbum.Domain;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Models.Identity;
using MyAlbum.Shared.Enums;

namespace MyAlbum.Application.Identity.implement
{
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly IEmployeeAccountReadService _employeeAccountReadService;
        private readonly IPasswordHasher<AccountDto> _hasher;
        public IdentityService(IAlbumDbContextFactory factory, IEmployeeAccountReadService employeeAccountReadService, IPasswordHasher<AccountDto> hasher) : base(factory)
        {
            _employeeAccountReadService = employeeAccountReadService;
            _hasher = hasher;
        }

        public async Task<EmpAccountDto> Login(LoginReq req)
        {
            EmpAccountDto result = new EmpAccountDto()
            {
                IsLoginSuccess = false
            };
            GetEmployeeReq getEmployeeReq = new GetEmployeeReq()
            {
                LoginName = req.LoginName
            };
            var accountResp = await _employeeAccountReadService.GetEmployeeAsync(getEmployeeReq);
            if (accountResp == null || accountResp.StatusCode != (long)ReturnCode.Succeeded)
                return result;
            if(accountResp.Data==null)
                return result;
            var account = accountResp.Data;

            if (account.IsActive == false)
                return result;
            var verify = _hasher.VerifyHashedPassword(account, account.PasswordHash, req.Password);
            if (verify == PasswordVerificationResult.Failed)
            {
                return result;
            }
            result.IsLoginSuccess = true;

            result.AccountId = account.AccountId;
            result.FullName = account.FullName;
            result.UserType = account.UserType;
            return result;
        }
    }
}

