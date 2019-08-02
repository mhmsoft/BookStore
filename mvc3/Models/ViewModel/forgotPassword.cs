using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mvc3.Models.ViewModel
{
    public class forgotPassword
    {
        [Display(Name ="Email")]
        [Required(ErrorMessage ="Boş bırakmayıznız")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Geçeri bir email adresi giriniz")]
        public string email { get; set; }
    }
}