//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace chatting.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public group()
        {
            this.usergroups = new HashSet<usergroups>();
            this.groupmessage = new HashSet<groupmessage>();
        }
    
        public int gid { get; set; }
        public int groupkatılım { get; set; }
        public string groupname { get; set; }
        public string groupdescription { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<usergroups> usergroups { get; set; }
        public virtual groupmessage groupmessage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<groupmessage> groupmessage1 { get; set; }
    }
}
