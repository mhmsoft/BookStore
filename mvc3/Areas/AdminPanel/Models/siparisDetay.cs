//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mvc3.Areas.AdminPanel.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class siparisDetay
    {
        public int Id { get; set; }
        public int siparisNo { get; set; }
        public int urunNo { get; set; }
        public Nullable<int> miktar { get; set; }
    
        public virtual siparis siparis { get; set; }
        public virtual urun urun { get; set; }
    }
}
