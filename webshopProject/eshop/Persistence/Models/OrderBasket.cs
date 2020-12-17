using System;
using System.Collections.Generic;

namespace eshop.Persistence.Models
{
    public partial class OrderBasket
    {
        public OrderBasket()
        {
            BasketProduct = new HashSet<BasketProduct>();
        }

        public int Id { get; set; }
        public int? OrderId { get; set; }

        public virtual Order Order { get; set; }
        public virtual ICollection<BasketProduct> BasketProduct { get; set; }
    }
}
