using System;
using System.Collections.Generic;

namespace eshop.Persistence.Models
{
    public partial class ProductStockAudit
    {
        public int? ProductId { get; set; }
        public string OldStock { get; set; }
        public string NewStock { get; set; }
        public DateTime? Date { get; set; }
    }
}
