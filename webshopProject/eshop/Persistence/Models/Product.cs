using System;
using System.Collections.Generic;

namespace eshop.Persistence.Models
{
    public partial class Product
    {
        public Product()
        {
            BasketProduct = new HashSet<BasketProduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte? Discount { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public int BrandId { get; set; }
        public string ImageUrl { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category CategoryNavigation { get; set; }
        public virtual ICollection<BasketProduct> BasketProduct { get; set; }
    }
}
