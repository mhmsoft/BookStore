using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc3.Models.ViewModel
{
    public class AccountVM
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string gsm { get; set; }
        public string newPassword { get; set; }
        public string comfirmPassword { get; set; }
        public bool subscribe { get; set; }
    }
}