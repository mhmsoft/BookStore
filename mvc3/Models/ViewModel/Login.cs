using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mvc3.Models.ViewModel
{
    public class Login
    {
        [Display(Name ="Kullanıcı Adı")]
        [Required(ErrorMessage ="Boş geçilemez")]
        public string email { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Boş geçilemez")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool rememberMe { get; set; }
    }

}