using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace OnDemandService.Models
{
    public static class PincodeStaticModel
    {
        public static List<Pincode> lstStaticPincodes { get; set; }
        public static List<Category> lstStaticCategories { get; set; }
        public static List<SubCategory> lstStaticSubcategories { get; set; }
    }
}