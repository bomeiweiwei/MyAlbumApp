using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlbum.Models.Member
{
    public class MemberCreate
    {
        public Guid AccountId { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
