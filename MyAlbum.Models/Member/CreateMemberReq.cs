using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlbum.Models.Member
{
    public class CreateMemberReq
    {
        public string LoginName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid? OperatorId { get; set; }
    }
}
