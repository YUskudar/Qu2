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
    
    public partial class directmessage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public directmessage()
        {
            this.dmuser = new HashSet<dmuser>();
        }
    
        public int dmid { get; set; }
        public string message { get; set; }
        public byte[] msdate { get; set; }
        public int dm_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dmuser> dmuser { get; set; }
    }
}
