using System.ComponentModel.DataAnnotations.Schema;

namespace EventSourcingPattern.QueryEventsApi.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<OrderItem> OrderedItems { get; set; }
    }
}
