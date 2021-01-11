using System;
using System.Collections.Generic;

namespace webshop.Models
{
    public partial class ProductOutOfStockNotification
    {
        public int ProductStockId { get; set; }
        public int ProductId { get; set; }
        public DateTime? Date { get; set; }
    }
}
