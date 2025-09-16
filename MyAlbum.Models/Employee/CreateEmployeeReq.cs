using System;
namespace MyAlbum.Models.Employee
{
	public class CreateEmployeeReq
	{
		public string LoginName { get; set; }
		public string FullName { get; set; }
		public string Title { get; set; }
        public string Email { get; set; }
		public string Password { get; set; }
		public string PasswordHash { get; set; } = string.Empty;
		public DateOnly HireDate { get; set; }
		public Guid? OperatorId { get; set; }
    }
}

