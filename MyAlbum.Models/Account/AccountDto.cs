using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlbum.Models.Account
{
    public class AccountDto
    {
        public Guid AccountId { get; set; }
        public string? LoginName { get; set; }
        public bool IsActive { get; set; }
        public string UserType { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public int EmployeeId { get; set; }
        public string Title { get; set; }
        public DateOnly? HireDate { get; set; }
        public string Email { get; set; }
    }
}
