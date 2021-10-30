using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnDemandService.Models
{
    public class NotificationModel
    {
        public int OrderId { get; set; }
        public int PartnerId { get; set; }
        public int CustomerId { get; set; }
        public string Partnername { get; set; }
        public string Customername { get; set; }
        public string Service { get; set; }
        public string PartnerMobileno { get; set; }
        public string CustomerMobileno { get; set; }
        public string PartnerPincode { get; set; }
        public string CustomerPincode { get; set; }
        public string Partneraddress { get; set; }
        public string Customeraddress { get; set; }
        public string Notification { get; set; }
        public int OrderRequestid { get; set; }
        public int count { get; set; }

        public string AppointedDate { get; set; }

        public string Notificationtype { get; set; }  //PartnerApproved (Partner first time date providing) , Newlead-Newlead for Partner, Approved, Accepted
    }
}