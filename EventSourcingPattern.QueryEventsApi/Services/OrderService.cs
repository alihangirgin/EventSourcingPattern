using EventSourcingPattern.QueryEventsApi.Dtos;
using EventSourcingPattern.QueryEventsApi.Models;
using EventSourcingPattern.Shared.Events;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingPattern.QueryEventsApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApiDbContext _context;
        private readonly IProductService _productService;

        public OrderService(ApiDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public async Task CreateOrder (OrderCreated orderCreated)
        {
            Order order = new()
            {
                Id = orderCreated.Id,
                CreatedAt = DateTime.UtcNow,
                OrderItems = orderCreated.OrderItems.Select(x=> new OrderItem()
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                }).ToList()
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            await _productService.UpdateProductStock(order.OrderItems.ToList());
        }

        public async Task<List<OrderDto>> GetOrders()
        {
            return await _context.Orders.Select(x => new OrderDto()
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                OrderItems = x.OrderItems.Select(y=> new OrderItemDto()
                {
                    Id = y.Id,
                    ProductId = y.ProductId,
                    Quantity = y.Quantity,
                }).ToList(),
            }).ToListAsync();
        }
    }
}
