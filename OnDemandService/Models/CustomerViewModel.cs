using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnDemandService.Models
{
    public class CustomerViewModel
    {
        public string Customerid { get; set; }
        public string CustomerEmail { get; set; }
    }

    public class UserViewModelOrder
    {
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string Customername { get; set; }
        public string Mobileno { get; set; }
        public string Useremail { get; set; }

    }
}