using System;
using System.Collections.Generic;

namespace eshop.Persistence.Models
{
    public partial class BasketProduct
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual OrderBasket Basket { get; set; }
        public virtual Product Product { get; set; }
    }
}
