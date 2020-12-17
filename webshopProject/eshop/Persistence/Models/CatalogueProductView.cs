using System;
using System.Collections.Generic;

namespace eshop.Persistence.Models
{
    public partial class CatalogueProductView
    {
        public string Category { get; set; }
        public string ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte? Discount { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public string BrandDescription { get; set; }
    }
}
