using System;
using System.Collections.Generic;

namespace webshop.Models
{
    public partial class ProductBasketJoint
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
