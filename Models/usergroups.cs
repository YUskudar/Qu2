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
    
    public partial class usergroups
    {
        public int ugid { get; set; }
        public int user_id { get; set; }
        public int group_id { get; set; }
    
        public virtual group group { get; set; }
        public virtual user user { get; set; }
    }
}
