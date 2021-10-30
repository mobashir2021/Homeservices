using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnDemandService.Models
{
    public class CategoryViewModel
    {
        
        public Category Category { get; set; }
        public List<SubCategory> subCategories { get; set; }
    }

    public class CategoryVM
    {
        public int CategoryId { get; set; }
        public string Categoryname { get; set; }
        public string Categorypath { get; set; }

        public List<SubcategoriesVM> subCategories { get; set; }
    }
}