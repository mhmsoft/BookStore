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
    
        public virtual user user { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<siparisDetay> siparisDetay { get; set; }
    }
}
