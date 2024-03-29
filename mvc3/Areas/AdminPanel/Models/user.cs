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
    
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            this.favorim = new HashSet<favorim>();
            this.siparis = new HashSet<siparis>();
            this.indirim = new HashSet<indirim>();
        }
    
        public int userId { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string rePassword { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string activationCode { get; set; }
        public string resetCode { get; set; }
        public string hostName { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<int> loginAttempt { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public Nullable<System.DateTime> loginTime { get; set; }
        public Nullable<bool> isMailVerified { get; set; }
        public Nullable<int> roleId { get; set; }
        public Nullable<bool> subscribe { get; set; }
        public string city { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<favorim> favorim { get; set; }
        public virtual role role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<siparis> siparis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<indirim> indirim { get; set; }
    }
}
