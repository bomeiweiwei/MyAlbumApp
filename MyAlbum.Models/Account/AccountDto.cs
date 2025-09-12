using System;
namespace MyAlbum.Models.Account
{
	public class AccountDto
	{
        public Guid AccountId { get; set; }
        public string? LoginName { get; set; }
        public bool IsActive { get; set; }
        public string UserType { get; set; }
        public string FullName { get; set; }
    }
}

