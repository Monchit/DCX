//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DocCtrlX.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class td_doc
    {
        public td_doc()
        {
            this.td_distribution = new HashSet<td_distribution>();
            this.td_tran = new HashSet<td_tran>();
        }
    
        public string doc_no { get; set; }
        public string doc_name { get; set; }
        public Nullable<int> doc_rev { get; set; }
        public byte doc_type_id { get; set; }
        public string cust_no { get; set; }
        public string change_point { get; set; }
        public System.DateTime eff_date { get; set; }
        public System.DateTime receive_date { get; set; }
        public string attach { get; set; }
    
        public virtual ICollection<td_distribution> td_distribution { get; set; }
        public virtual tm_doc_type tm_doc_type { get; set; }
        public virtual ICollection<td_tran> td_tran { get; set; }
    }
}
