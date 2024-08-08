using System.Text;
using EventStore.Client;
using System.Text.Json;

namespace EventSourcingPattern.CommandEventsApi.Services
{
    public class EventStoreService : IEventStoreService
    {
        private readonly EventStoreClientSettings _settings;
        public EventStoreService(string connectionString)
        {
            _settings = EventStoreClientSettings.Create(connectionString);

        }
        public async Task AppendEventToStreamAsync(string streamName, object eventData, string eventType)
        {
            try
            {
                var client = new EventStoreClient(_settings);

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
                await client.AppendToStreamAsync(streamName, StreamState.Any, new[] { eventDataEvent });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while appending the event: {ex.Message}");
            }
        }
    }
}
