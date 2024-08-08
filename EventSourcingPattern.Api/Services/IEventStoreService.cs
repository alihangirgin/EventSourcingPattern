namespace EventSourcingPattern.Api.Services
{
    public interface IEventStoreService
    {
        Task AppendEventToStreamAsync(string streamName, object eventData, string eventType);
        //Task SubscribeToStreamAsync(string streamName, CancellationToken ct);
    }
}
