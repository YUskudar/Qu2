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
    
    public partial class video
    {
        public int vid { get; set; }
        public byte[] video1 { get; set; }
        public int content_id { get; set; }
    
        public virtual content content { get; set; }
    }
}
