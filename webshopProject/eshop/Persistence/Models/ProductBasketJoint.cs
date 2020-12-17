using System;
using System.Collections.Generic;

namespace eshop.Persistence.Models
{
    public partial class ProductBasketJoint
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
