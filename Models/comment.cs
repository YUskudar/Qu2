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
    
    public partial class comment
    {
        public int coid { get; set; }
        public string comment1 { get; set; }
        public System.DateTime date { get; set; }
        public int content_id { get; set; }
        public int user_id { get; set; }

        public virtual content content { get; set; }
        public virtual user user { get; set; }
    }
}
