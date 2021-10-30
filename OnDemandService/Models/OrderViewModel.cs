using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnDemandService.Models
{
    public class OrderViewModel
    {
        
        [Key]
        public int OrderId { get; set; }

        public int Partnerid { get; set; }
        public int Customerid { get; set; }
        public int SNo { get; set; }
        public string Service { get; set; }
        
        public string CustomerName { get; set; }
        
        public string AppointmentDate { get; set; }

        public string Status { get; set; }
        public string Amount { get; set; }
        public string Area { get; set; }

        public string PlacedOn { get; set; }

        public string StatusValue { get; set; }
        public string PartnerProfName { get; set; }

        public string Orderwithpartner { get; set; }

        public string PartnerphoneNumber { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string PartnerEmail { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }

        public int OrderRequestId { get; set; }

        public string Remarks { get; set; }

        public int Rating { get; set; }

        public bool isRatingDone { get; set; }

        public string Usercomments { get; set; }

        public int BillPaymentId { get; set; }

        public List<BillPaymentDetailsInnerVM> paymentDetails { get; set; }

        public string BillTotalPrice { get; set; }

        public string CGST { get; set; }
        public string SGST { get; set; }

        public string Notificationtype { get; set; }

    }

    public class BillPaymentDetailsInnerVM
    {
        public int SubcategoryId { get; set; }
        public string Subcategoryname { get; set; }
        public int quantity { get; set; }
        public string Price { get; set; }
    }
}