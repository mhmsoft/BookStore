using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc3.Models.ViewModel
{
    public class guest
    {
        public bool isGuest { get; set; }
        public string soyad { get; set; }
        public string ad { get; set; }
        public string adres { get; set; }
        public string sehir { get; set; }
        public string telefon { get; set; }
        public int postakodu { get; set; }
        public string sirket { get; set; }
    }
}