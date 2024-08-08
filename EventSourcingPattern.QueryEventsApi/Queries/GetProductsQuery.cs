using EventSourcingPattern.QueryEventsApi.Dtos;
using EventSourcingPattern.QueryEventsApi.Services;
using MediatR;

namespace EventSourcingPattern.QueryEventsApi.Queries
{
    public sealed record GetProductsQuery() : IRequest<List<ProductDto>>;

    public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IProductService _productService;

        public GetProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            return await _productService.GetProducts();
        }
    }
}
