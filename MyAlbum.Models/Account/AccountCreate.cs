using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlbum.Models.Account
{
    public class AccountCreate
    {
        public Guid AccountId { get; set; }
        public string LoginName { get; set; }
        public string NormalizedLoginName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public Guid SecurityStamp { get; set; }
        public byte UserType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
