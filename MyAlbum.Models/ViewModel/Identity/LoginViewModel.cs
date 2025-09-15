using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyAlbum.Models.ViewModel.Identity
{
	public class LoginViewModel
	{
        [Required, Display(Name = "帳號")]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "密碼")]
        public string Password { get; set; }

        [Display(Name = "記住我")]
        public bool RememberMe { get; set; }
    }
}

