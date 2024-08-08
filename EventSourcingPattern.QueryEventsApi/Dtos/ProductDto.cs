using System.ComponentModel.DataAnnotations.Schema;
using EventSourcingPattern.QueryEventsApi.Models;

namespace EventSourcingPattern.QueryEventsApi.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
