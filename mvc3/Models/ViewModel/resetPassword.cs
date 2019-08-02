using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mvc3.Models.ViewModel
{
    public class resetPassword
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email adresi girmeniz gerekir")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "parola girmeniz gerekir")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minumum 6 karakter girmeniz gerekir")]
        public string newPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "parolanız öncekiyle eşleşmiyor.")]
        public string comfirmPassword { get; set; }
        public string resetCode { get; set; }
    }
}