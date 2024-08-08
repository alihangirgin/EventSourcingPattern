using EventSourcingPattern.QueryEventsApi.Dtos;
using EventSourcingPattern.QueryEventsApi.Models;
using EventSourcingPattern.Shared.Events;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingPattern.QueryEventsApi.Services
{
    public class ProductService : IProductService
    {
        private readonly ApiDbContext _context;

        public ProductService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task CreateProduct(ProductCreated productCreated)
        {
            Product product = new()
            {
                Id = productCreated.Id,
                Name = productCreated.Name,
                Price = productCreated.Price,
                Stock = productCreated.Stock,
                CreatedAt = DateTime.Now
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateProductName(ProductNameUpdated productNameUpdated)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productNameUpdated.Id);
            if (product == null) return;
            product.Name = productNameUpdated.Name;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateProductPrice(ProductPriceUpdated productPriceUpdated)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productPriceUpdated.Id);
            if (product == null) return;
            product.Price = productPriceUpdated.Price;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductStock(List<OrderItem> orderedItems)
        {
            var products = await _context.Products
                .Where(x => orderedItems.Select(y => y.ProductId).ToList().Distinct().Contains(x.Id))
                .ToListAsync();
            if (products.Count == 0) return;

            foreach (var orderedItem in orderedItems)
            {
                var product = products.FirstOrDefault(x => x.Id == orderedItem.ProductId);
                if (product == null) continue;
                product.Stock -= orderedItem.Quantity;
            }
            _context.Products.UpdateRange(products);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            return await _context.Products
                .Select(x => new ProductDto()
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Name = x.Name,
                    Price = x.Price,
                    Stock = x.Stock
                }).ToListAsync();
        }
    }
}
