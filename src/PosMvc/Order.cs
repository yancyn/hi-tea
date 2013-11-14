//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PosMvc
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
    
        public int Id { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime Created { get; set; }
        public Nullable<System.DateTime> ReceiptDate { get; set; }
        public string QueueNo { get; set; }
        public string TableNo { get; set; }
        public Nullable<int> MemberId { get; set; }
        public decimal Total { get; set; }
    
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual User Staff { get; set; }
        public virtual User Member { get; set; }
    }
}
