using EventStore.Client;
using System.Text.Json;
using System.Text;
using EventSourcingPattern.Shared.Events;

namespace EventSourcingPattern.QueryEventsApi.Services
{
    public class EventStoreService : IEventStoreService
    {
        private readonly EventStoreClientSettings _settings;
        private readonly IServiceProvider _serviceProvider;
        public EventStoreService(string connectionString, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _settings = EventStoreClientSettings.Create(connectionString);
        }

        public async Task SubscribeToAllStreamsAsync()
        {
            var client = new EventStoreClient(_settings);

            var start = FromAll.End;

            Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared =
                async (subscription, resolvedEvent, cancellationToken) =>
                {
                    using var scope = _serviceProvider.CreateScope(); 
                    var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
                    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                    var eventData = resolvedEvent.Event;
                    var eventDataJson = Encoding.UTF8.GetString(eventData.Data.ToArray());
                    var eventType = eventData.EventType;

                    switch (eventType)
                    {
                        case "ProductCreated":
                            Console.WriteLine("ProductCreated");
                            var productCreated = JsonSerializer.Deserialize<ProductCreated>(eventDataJson);
                            if (productCreated == null) return;
                            await productService.CreateProduct(productCreated);
                            return;
                        case "ProductNameUpdated":
                            Console.WriteLine("ProductNameUpdated");
                            var productNameUpdated = JsonSerializer.Deserialize<ProductNameUpdated>(eventDataJson);
                            if (productNameUpdated == null) return;
                            await productService.UpdateProductName(productNameUpdated);
                            return;
                        case "ProductPriceUpdated":
                            Console.WriteLine("ProductPriceUpdated");
                            var productPriceUpdated = JsonSerializer.Deserialize<ProductPriceUpdated>(eventDataJson);
                            if (productPriceUpdated == null) return;
                            await productService.UpdateProductPrice(productPriceUpdated);
                            return;
                        case "OrderCreated":
                            Console.WriteLine("OrderCreated");
                            var orderCreated = JsonSerializer.Deserialize<OrderCreated>(eventDataJson);
                            if (orderCreated == null) return;
                            await orderService.CreateOrder(orderCreated);
                            return;
                    }

                    Console.WriteLine($"Event received: {eventDataJson}");
                    Console.WriteLine($"Event received: {eventType}");

                    // Event'i işleme
                    // Örneğin: Deserialize, veritabanına kaydetme, vb.
                };


            Action<StreamSubscription, SubscriptionDroppedReason, Exception?> subscriptionDropped =
                (subscription, reason, exception) =>
                {
                    Console.WriteLine($"Subscription dropped: {reason}");
                    if (exception != null)
                    {
                        Console.WriteLine($"Exception: {exception.Message}");
                    }
                };

            
            await client.SubscribeToAllAsync(
                start,
                eventAppeared,
                resolveLinkTos: true,
                subscriptionDropped: subscriptionDropped
            );
        }
    }
}
