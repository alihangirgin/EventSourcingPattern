using System.Text;
using EventStore.Client;
using System.Text.Json;
using System.Linq;

namespace EventSourcingPattern.Api.Services
{
    public class EventStoreService : IEventStoreService
    {
        private readonly EventStoreClientSettings _settings;
        //private readonly EventStoreClient _client;
        public EventStoreService(string connectionString)
        {
            _settings = EventStoreClientSettings.Create(connectionString);
            //_client = new EventStoreClient(_settings);

        }
        public async Task AppendEventToStreamAsync(string streamName, object eventData, string eventType)
        {
            try
            {

                var _client = new EventStoreClient(_settings);

                HashSet<Guid> processedEvents = new HashSet<Guid>();

                //var start = FromStream.After(lastPosition);
                var start = FromAll.End;

                // Geri çağırma işlevini oluşturun
                Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared =
                    async (subscription, resolvedEvent, cancellationToken) =>
                    {
                        var eventData = resolvedEvent.Event;
                        var eventDataJson = Encoding.UTF8.GetString(eventData.Data.ToArray());
                        var eventType = eventData.EventType;
                        var eventId = eventData.EventId.ToGuid();

                        if (processedEvents.Contains(eventData.EventId.ToGuid()))
                        {
                            // Daha önce işlenmiş olay, işlemi atla
                            Console.WriteLine($"Çııık");
                            return;
                        }

                        processedEvents.Add(eventData.EventId.ToGuid());
                        Console.WriteLine($"Event received: {eventDataJson}");
                        Console.WriteLine($"Event received: {eventType}");
                        Console.WriteLine($"Event Id: {eventId.ToString()}");

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
                await _client.SubscribeToAllAsync(
                    start,
                    eventAppeared,
                    resolveLinkTos: true, // Link'leri çözümlemek isteyip istemediğinize göre ayarlayın
                    subscriptionDropped: subscriptionDropped
                );


                //// Abonelik yap
                //await _client.SubscribeToStreamAsync(
                //    streamName,
                //    start,
                //    eventAppeared,
                //    resolveLinkTos: true, // Link'leri çözümlemek isteyip istemediğinize göre ayarlayın
                //    subscriptionDropped: subscriptionDropped
                //);





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

        //private async Task<StreamPosition> GetLastEventPositionAsync(EventStoreClient client, string streamName)
        //{
        //    var result = _client.ReadStreamAsync(
        //        Direction.Backwards,
        //        streamName,
        //        StreamPosition.End,
        //        1);

        //    if (await result.ReadState == ReadState.StreamNotFound)
        //    {
        //        return StreamPosition.End;
        //    }


        //    ResolvedEvent @event = await result.FirstOrDefaultAsync();
        //    StreamPosition position = @event.OriginalEventNumber;
        //    return position;
        //}

        //public async Task SubscribeToStreamAsync(string streamName, CancellationToken ct)
        //{

        //    //await using var client = new EventStoreClient(_settings);

        //    await using var subscription = _client.SubscribeToStream(
        //        streamName,
        //        FromStream.Start,
        //        cancellationToken: ct);





        //    await foreach (var message in subscription.Messages.WithCancellation(ct))
        //    {
        //        switch (message)
        //        {
        //            case StreamMessage.Event(var evnt):
        //                Console.WriteLine($"Received event {evnt.OriginalEventNumber}@{evnt.OriginalStreamId}");
        //                //await HandleEvent(evnt);
        //                break;
        //        }
        //    }
        //}

    }
}
