using System;
using MyAlbum.Models.Account;
using MyAlbum.Models.Identity;

namespace MyAlbum.Application.Identity
{
	public interface IIdentityService
	{
        Task<EmpAccountDto> Login(LoginReq req);
    }
}

