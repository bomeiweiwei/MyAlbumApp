using System;
using System.ComponentModel.DataAnnotations;
namespace MyAlbum.Models.Employee
{
	public class CreateEmployeeReq
	{
        [Required]
        public string LoginName { get; set; }
        [Required]
        public string FullName { get; set; }
		public string Title { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
		public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public DateOnly HireDate { get; set; }
		public Guid? OperatorId { get; set; }
    }
}

