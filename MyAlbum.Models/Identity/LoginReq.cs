using System;
using System.ComponentModel.DataAnnotations;

namespace MyAlbum.Models.Identity
{
	public class LoginReq
	{
        [Required]
        public string LoginName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

