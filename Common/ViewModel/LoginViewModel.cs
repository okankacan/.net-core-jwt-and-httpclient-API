using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Common.ViewModel
{
    public class LoginViewModel
    {
       
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "ZLA.AccountSite.UserNameInvalid")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
