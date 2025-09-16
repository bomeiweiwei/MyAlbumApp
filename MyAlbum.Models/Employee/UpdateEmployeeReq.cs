using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlbum.Models.Employee
{
    public class UpdateEmployeeReq 
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; } = string.Empty;
        public string NewPasswordHash { get; set; } = string.Empty;
        public DateOnly? HireDate { get; set; }
        public Guid? OperatorId { get; set; }
    }
}
