using EventStore.ClientAPI;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EventSourcingCQRS.Domain.EventStore.Tests
{
    [Trait("Type", "Integration")]
    public class EventStoreTest : IDisposable
    {
        private IEventStoreConnection connection;
        private string stream;

        public EventStoreTest()
        {
            connection = EventStoreConnection.Create(new Uri("tcp://localhost:1113"));
            connection.ConnectAsync().Wait();
            stream = Guid.NewGuid().ToString();
        }

        [Fact]
        public async Task TestStreamDoesNotExists()
        {
            var events = await connection.ReadStreamEventsForwardAsync(stream, StreamPosition.Start, 1, false);

            Assert.Equal(SliceReadStatus.StreamNotFound, events.Status);
        }

        [Fact]
        public async Task TestStreamExists()
        {
            await AppendEventToStreamAsync();

            var events = await connection.ReadStreamEventsForwardAsync(stream, StreamPosition.Start, 1, false);

            Assert.Equal(SliceReadStatus.Success, events.Status);
            Assert.Single(events.Events);
        }

        [Fact]
        public async Task TestPerformance()
        {
            for (int i = 0; i < 100; i++)
            {
                await connection.AppendToStreamAsync(stream, i - 1,
                    new EventData(Guid.NewGuid(), "test", true, Encoding.UTF8.GetBytes("{}"), StreamMetadata.Create().AsJsonBytes()));
            }
        }

        private async Task AppendEventToStreamAsync()
        {
            await connection.AppendToStreamAsync(stream, ExpectedVersion.NoStream,
                new EventData(Guid.NewGuid(), "test", true, Encoding.UTF8.GetBytes("{}"), StreamMetadata.Create().AsJsonBytes()));
        }

        public void Dispose()
        {
            connection.DeleteStreamAsync(stream, ExpectedVersion.Any).Wait();
            connection.Dispose();
        }
    }
}