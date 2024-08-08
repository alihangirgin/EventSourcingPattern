using EventSourcingPattern.QueryEventsApi.Services;

namespace EventSourcingPattern.QueryEventsApi.BackgroundServices
{

    public class EventStoreConsumeBackgroundService : BackgroundService
    {
        private readonly IEventStoreService _eventStoreService;

        public EventStoreConsumeBackgroundService(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await _eventStoreService.SubscribeToAllStreamsAsync();
        }
    }
}
