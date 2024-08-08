using EventSourcingPattern.QueryEventsApi.Dtos;
using EventSourcingPattern.QueryEventsApi.Models;
using EventSourcingPattern.Shared.Events;

namespace EventSourcingPattern.QueryEventsApi.Services
{
    public interface IProductService
    {
        Task CreateProduct(ProductCreated productCreated);
        Task UpdateProductName(ProductNameUpdated productNameUpdated);
        Task UpdateProductPrice(ProductPriceUpdated productPriceUpdated);
        Task UpdateProductStock(List<OrderItem> orderedItems);
        Task<List<ProductDto>> GetProducts();
    }
}
