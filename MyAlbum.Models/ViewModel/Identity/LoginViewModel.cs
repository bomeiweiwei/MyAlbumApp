using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyAlbum.Models.ViewModel.Identity
{
	public class LoginViewModel
	{
        [Required(ErrorMessage = "請輸入帳號"), Display(Name = "帳號")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "請輸入密碼"), DataType(DataType.Password), Display(Name = "密碼")]
        public string Password { get; set; }

        [Display(Name = "記住我")]
        public bool RememberMe { get; set; }
    }
}

