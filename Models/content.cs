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
    
    public partial class content
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public content()
        {
            this.picture = new HashSet<picture>();
        }
    
        public int cid { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public int user_id { get; set; }
        public Nullable<System.DateTime> date { get; set; }
    
        public virtual user user { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<picture> picture { get; set; }
    }
}
