namespace EventSourcingPattern.QueryEventsApi.Services
{
    public interface IEventStoreService
    {
        Task SubscribeToAllStreamsAsync();
    }
}
