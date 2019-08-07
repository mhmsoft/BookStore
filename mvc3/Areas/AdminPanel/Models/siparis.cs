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
    
    public partial class siparis
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public siparis()
        {
            this.siparisDetay = new HashSet<siparisDetay>();
        }
    
        public int siparisNo { get; set; }
        public Nullable<int> musteriNo { get; set; }
        public Nullable<System.DateTime> siparisTarihi { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string company { get; set; }
        public string city { get; set; }
        public Nullable<int> postakodu { get; set; }
        public string email { get; set; }
        public string note { get; set; }
        public Nullable<decimal> gonderimtutar { get; set; }
        public Nullable<bool> farkliadres { get; set; }
        public Nullable<decimal> indirimtutar { get; set; }
        public Nullable<decimal> siparistutar { get; set; }
        public virtual user user { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<siparisDetay> siparisDetay { get; set; }
    }
}
