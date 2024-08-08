using System.Text;
using EventStore.Client;
using System.Text.Json;

namespace EventSourcingPattern.CommandEventsApi.Services
{
    public class EventStoreService : IEventStoreService
    {
        private readonly EventStoreClientSettings _settings;
        private readonly EventStoreClient _client;
        public EventStoreService(string connectionString)
        {
            _settings = EventStoreClientSettings.Create(connectionString);
            _client = new EventStoreClient(_settings);

        }
        public async Task AppendEventToStreamAsync(string streamName, object eventData, string eventType)
        {
            try
            {

                var start = FromStream.End;

                // Geri çağırma işlevini oluşturun
                Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared =
                    async (subscription, resolvedEvent, cancellationToken) =>
                    {
                        var eventData = resolvedEvent.Event;
                        var eventDataJson = Encoding.UTF8.GetString(eventData.Data.ToArray());
                        var eventType = eventData.EventType;
                        Console.WriteLine("hebele");
                        Console.WriteLine($"Event received: {eventDataJson}");
                        Console.WriteLine($"Event received: {eventType}");

                        // Event'i işleme
                        // Örneğin: Deserialize, veritabanına kaydetme, vb.
                    };

                // Abonelik işlevi oluşturun
                Action<StreamSubscription, SubscriptionDroppedReason, Exception?> subscriptionDropped =
                    (subscription, reason, exception) =>
                    {
                        Console.WriteLine($"Subscription dropped: {reason}");
                        if (exception != null)
                        {
                            Console.WriteLine($"Exception: {exception.Message}");
                        }
                    };

                // Abonelik yap
                await _client.SubscribeToStreamAsync(
                    streamName,
                    start,
                    eventAppeared,
                    resolveLinkTos: true, // Link'leri çözümlemek isteyip istemediğinize göre ayarlayın
                    subscriptionDropped: subscriptionDropped
                );





                //await using var client = new EventStoreClient(_settings);

                // Serialize the event data to JSON
                var eventDataJson = JsonSerializer.Serialize(eventData);
                var eventDataBytes = System.Text.Encoding.UTF8.GetBytes(eventDataJson);

                // Create the event
                var eventDataEvent = new EventData(
                    Uuid.NewUuid(), // Generate a new UUID for the event
                    eventType,
                    eventDataBytes
                );

                // Append the event to the stream
                await _client.AppendToStreamAsync(streamName, StreamState.Any, new[] { eventDataEvent });




            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while appending the event: {ex.Message}");
            }
        }

        public async Task SubscribeToStreamAsync(string streamName, CancellationToken ct)
        {

            //await using var client = new EventStoreClient(_settings);

            await using var subscription = _client.SubscribeToStream(
                streamName,
                FromStream.Start,
                cancellationToken: ct);





            await foreach (var message in subscription.Messages.WithCancellation(ct))
            {
                switch (message)
                {
                    case StreamMessage.Event(var evnt):
                        Console.WriteLine($"Received event {evnt.OriginalEventNumber}@{evnt.OriginalStreamId}");
                        //await HandleEvent(evnt);
                        break;
                }
            }
        }

    }
}
