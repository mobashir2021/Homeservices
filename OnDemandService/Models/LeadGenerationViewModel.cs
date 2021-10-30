﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnDemandService.Models
{
    public class LeadGenerationViewModel
    {

        public int Leadgenerationid { get; set; }

        public int CustomerId { get; set; }
        public string Customername { get; set; }

        public string Customermobileno { get; set; }

        public string Customeraddress { get; set; }

        public int PartnerId { get; set; }
        public string Partnername { get; set; }

        public string Partnermobileno { get; set; }

        public string Partneraddress { get; set; }

        public int UserRating { get; set; }
        public string UserComment { get; set; }

        public string Orderdesc { get; set; }

        public int Serviceprovided { get; set; }

        public DateTime Orderdatetime { get; set; }

        public string AppointedDate { get; set; }

        public int Cityid { get; set; }

        public int Pincode { get; set; }

        public string LeadStatus { get; set; }

        public string City { get; set; }
        public string ServiceName { get; set; }

        public int CreditUsed { get; set; }
    }
}