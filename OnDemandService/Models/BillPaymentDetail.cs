//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnDemandService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BillPaymentDetail
    {
        public int BillPaymentDetailsId { get; set; }
        public int BillPaymentId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SubCategoryId { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Comment { get; set; }
        public string Price { get; set; }
        public Nullable<int> Quantity { get; set; }
    }
}
