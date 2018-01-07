using EventSourcingCQRS.Domain.Persistence;
using EventSourcingCQRS.Domain.Persistence.EventStore;
using EventSourcingCQRS.Domain.PubSub;
using EventSourcingCQRS.Domain.Tests.Utility;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EventSourcingCQRS.Domain.Tests
{
    [Trait("Type", "Unit")]
    public partial class EventSourcingRepositoryTest
    {
        private IRepository<TestAggregate, TestAggregateId> sut;
        private Mock<IEventStore> eventStoreMock;
        private Mock<ITransientDomainEventPublisher> domainEventPublisherMock;
        private static readonly TestAggregateId DefaultId = new TestAggregateId();

        public EventSourcingRepositoryTest()
        {
            domainEventPublisherMock = new Mock<ITransientDomainEventPublisher>();
            eventStoreMock = new Mock<IEventStore>();
            sut = new EventSourcingRepository<TestAggregate, TestAggregateId>(eventStoreMock.Object, domainEventPublisherMock.Object);
        }

        [Fact]
        public async Task ShouldLoadAnAggregateAndApplyEventsAsync()
        {
            TestDomainEvent domainEvent = new TestDomainEvent();
            eventStoreMock.Setup(x => x.ReadEventsAsync(DefaultId))
                .ReturnsAsync(new List<Event<TestAggregateId>>()
                {
                    new Event<TestAggregateId>(domainEvent, 0)
                });
            var aggregate = await sut.GetByIdAsync(DefaultId);

            Assert.NotNull(aggregate);
            Assert.Single(aggregate.AppliedEvents);
            Assert.Equal(domainEvent, aggregate.AppliedEvents[0]);
        }

        [Fact]
        public async Task ShouldPublishUncommittedEventsOnSaveAsync()
        {
            TestDomainEvent domainEvent = new TestDomainEvent();
            var aggregate = new TestAggregate(domainEvent);

            eventStoreMock.Setup(x => x.AppendEventAsync(domainEvent)).ReturnsAsync(new AppendResult(1));
            await sut.SaveAsync(aggregate);
            domainEventPublisherMock.Verify(x => x.PublishAsync(domainEvent));
        }

        [Fact]
        public async Task ShouldReturnsNullWhenAggregateNotFoundOrDeletedAsync()
        {
            eventStoreMock.Setup(x => x.ReadEventsAsync(DefaultId)).Throws<EventStoreAggregateNotFoundException>();
            Assert.Null(await sut.GetByIdAsync(DefaultId));
        }

        [Fact]
        public void ShouldThrowsExceptionWhenEventStoreHasCommunicationIssues()
        {
            eventStoreMock.Setup(x => x.ReadEventsAsync(DefaultId)).Throws<EventStoreCommunicationException>();
            Assert.ThrowsAsync<RepositoryException>(async () => { await sut.GetByIdAsync(DefaultId); });
        }
    }
}
