//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class @class
    {
        public @class()
        {
            this.sxc = new HashSet<sxc>();
        }

        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string description { get; set; }
    
        public virtual ICollection<sxc> sxc { get; set; }
    }
}
