using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingPattern.Shared.Events
{
    public class ProductPriceUpdated : IEvent
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
