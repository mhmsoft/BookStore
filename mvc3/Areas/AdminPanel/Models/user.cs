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
        public user()
        {
            this.Favorim = new HashSet<Favorim>();
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
        public string city { get; set; }
        public Nullable<bool> subscribe { get; set; }
    
        public virtual role role { get; set; }
        public virtual ICollection<Favorim> Favorim { get; set; }
    }
}