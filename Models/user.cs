//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Qu2SM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            this.comment = new HashSet<comment>();
            this.content = new HashSet<content>();
            this.groupmessage = new HashSet<groupmessage>();
            this.usergroups = new HashSet<usergroups>();
            this.likes = new HashSet<likes>();
        }
    
        public int userid { get; set; }
        public string useremail { get; set; }
        public string usernickname { get; set; }
        public string username { get; set; }
        public string usersurname { get; set; }
        public Nullable<int> userage { get; set; }
        public string usercity { get; set; }
        public Nullable<int> userphone { get; set; }
        public string userjob { get; set; }
        public string password { get; set; }
        public byte[] userpp { get; set; }
        public string userads { get; set; }
        public Nullable<int> role { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<content> content { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<groupmessage> groupmessage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<usergroups> usergroups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<likes> likes { get; set; }
        public virtual Roles Roles { get; set; }
    }
}
