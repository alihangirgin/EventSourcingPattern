using EventSourcingPattern.QueryEventsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingPattern.QueryEventsApi
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
                
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
