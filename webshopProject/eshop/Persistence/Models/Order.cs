using System;
using System.Collections.Generic;

namespace eshop.Persistence.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderBasket = new HashSet<OrderBasket>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; }
        public bool? IsPaid { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderBasket> OrderBasket { get; set; }
    }
}
