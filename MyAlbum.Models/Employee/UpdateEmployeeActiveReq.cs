using System;
namespace MyAlbum.Models.Employee
{
	public class UpdateEmployeeActiveReq
	{
        public int EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public Guid? OperatorId { get; set; }
    }
}

