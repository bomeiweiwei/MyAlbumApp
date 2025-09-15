using System;
namespace MyAlbum.Models.Employee
{
	public class EmployeeItem
	{
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = "";
        public string? Title { get; set; }
        public DateOnly? HireDate { get; set; }
        public bool IsActive { get; set; }
    }
}

