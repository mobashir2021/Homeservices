using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnDemandService.Models
{
    public class SubcategoriesVM
    {
        public int SubcategoryId { get; set; }
        public string Subcategoryname { get; set; }
        public int CategoryidForeign { get; set; }
        public string Subcategoryimage { get; set; }
        public string Price { get; set; }
    }
}