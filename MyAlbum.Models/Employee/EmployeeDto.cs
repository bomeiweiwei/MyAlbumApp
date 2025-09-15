using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlbum.Models.Employee
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public DateOnly? HireDate { get; set; }
    }
}
