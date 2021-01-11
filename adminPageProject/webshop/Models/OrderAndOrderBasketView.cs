using System;
using System.Collections.Generic;

namespace webshop.Models
{
    public partial class OrderAndOrderBasketView
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string Status { get; set; }
        public bool? IsPaid { get; set; }
        public int OrderBasketId { get; set; }
        public int ProductBasketJointId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
