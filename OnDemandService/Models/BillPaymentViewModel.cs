using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnDemandService.Models
{
    public class BillPaymentViewModel
    {
        public int BillPaymentId { get; set; }
        public int OrderId { get; set; }
        public string Customername { get; set; }
        public string Professionalname { get; set; }

        public string Categoryname { get; set; }
        public int Categoryid { get; set; }

        public int TotalPrice { get; set; }

        public int Customerid { get; set; }
        public int Professionalid { get; set; }
        public bool IsApproved { get; set; }
        public DateTime BillCreationDatetime { get; set; }
        public string Remarks { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

    public class BillPaymentDetailsViewModel
    {
        public int BillPaymentDetailsViewModelId { get; set; }
        public int Billpaymentid { get; set; }
        public int Subcategoryid { get; set; }
        public string Subcategoryname { get; set; }
        public int Price { get; set; }
        public int Rating { get; set; }
        public string Remarks { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
    }

    public class BillMainViewModel
    {
        public BillPaymentViewModel billPaymentViewModel { get; set; }
        public List<BillPaymentDetailsViewModel> childbillViewModel { get; set; }
    }

    public class BillOrderSelection
    {
        public int OrderId { get; set; }
        public string Ordername { get; set; }
    }

    public class BillOrderSubSelection
    {
        public string SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
    }
}