using System;
using System.Collections.Generic;

namespace eshop.Persistence.Models
{
    public partial class OrderAudit
    {
        public int? OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? CustomerId { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime? StatusChanged { get; set; }
    }
}
