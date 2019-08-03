using mvc3.Areas.AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc3.Models.ViewModel
{
    public class BasketItem
    {
        public Guid Id { get; set; }
        public urun product { get; set; }
        public int quantity { get; set; }
        public DateTime DateCreated { get; set; }

    }
}