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
    
    public partial class urun
    {
        public urun()
        {
            this.yorum = new HashSet<yorum>();
            this.resim = new HashSet<resim>();
            this.Favorim = new HashSet<Favorim>();
        }
    
        public int urunNo { get; set; }
        public string urunAdi { get; set; }
        public string yazar { get; set; }
        public string yayinEvi { get; set; }
        public Nullable<decimal> fiyat { get; set; }
        public string aciklama { get; set; }
        public Nullable<int> stok { get; set; }
        public Nullable<int> kategoriNo { get; set; }
    
        public virtual kategori kategori { get; set; }
        public virtual ICollection<yorum> yorum { get; set; }
        public virtual ICollection<resim> resim { get; set; }
        public virtual ICollection<Favorim> Favorim { get; set; }
    }
}