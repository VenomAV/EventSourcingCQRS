using EventSourcingCQRS.Application.PubSub;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EventSourcingCQRS.Application.Tests
{
    [Trait("Type", "Unit")]
    public class TransientDomainEventPublisherTest
    {
        private readonly Event publishedEvent = new Event();
        private TransientDomainEventPubSub sut;

        public TransientDomainEventPublisherTest()
        {
            sut = new TransientDomainEventPubSub();
        }

        [Fact]
        public async Task ShouldInvokeSubscriber()
        {
            //Arrange
            Event receivedEvent = null;
            sut.Subscribe<Event>(@event =>
            {
                receivedEvent = @event;
            });
            //Act
            await sut.PublishAsync(publishedEvent);
            //Assert
            Assert.Same(publishedEvent, receivedEvent);
        }

        [Fact]
        public async Task ShouldInvokeAllSubscribers()
        {
            //Arrange
            Event receivedEvent1 = null;
            Event receivedEvent2 = null;
            sut.Subscribe<Event>(@event =>
            {
                receivedEvent1 = @event;
            });
            sut.Subscribe<Event>(@event =>
            {
                receivedEvent2 = @event;
            });
            //Act
            await sut.PublishAsync(publishedEvent);
            //Assert
            Assert.Same(publishedEvent, receivedEvent1);
            Assert.Same(publishedEvent, receivedEvent2);
        }

        [Fact]
        public async Task ShouldNotInvokeSubscribersWhenTheEventIsNotOfTheExpectedType()
        {
            //Arrange
            Event receivedEvent = null;
            sut.Subscribe<Event>(@event =>
            {
                receivedEvent = @event;
            });
            //Act
            await sut.PublishAsync(new AnotherEvent());
            //Assert
            Assert.Null(receivedEvent);
        }

        [Fact]
        public async Task ShouldNotInvokeSubscribersOnceDispose()
        {
            //Arrange
            Event receivedEvent = null;
            sut.Subscribe<Event>(@event =>
            {
                receivedEvent = @event;
            });
            //Act
            sut.Dispose();
            await sut.PublishAsync(publishedEvent);
            //Assert
            Assert.Null(receivedEvent);
        }

        [Fact]
        public void ShouldNotInvokeSubscribersIfPublishIsOnADifferentScope()
        {
            Event receivedEvent = null;
            Task.WhenAll(Task.Run(() =>
            {
                sut.Subscribe<Event>(@event =>
                {
                    receivedEvent = @event;
                });
            }),
            Task.Run(async () => 
            {
                await sut.PublishAsync(publishedEvent);
            })
            ).Wait();
            //Assert
            Assert.Null(receivedEvent);
        }

        [Fact]
        public async Task ShouldInvokeAllSubscribersWhenAnyThrowsException()
        {
            //Arrange
            Event receivedEvent2 = null;
            sut.Subscribe<Event>(@event =>
            {
                throw new Exception("Failing operation");
            });
            sut.Subscribe<Event>(@event =>
            {
                receivedEvent2 = @event;
            });
            //Act
            await sut.PublishAsync(publishedEvent);
            //Assert
            Assert.Same(publishedEvent, receivedEvent2);
        }

        [Fact]
        public async Task ShouldWaitAsyncHandler()
        {
            //Arrange
            Event receivedEvent = null;
            sut.Subscribe<Event>(async @event =>
            {
                await Task.Delay(100);
                receivedEvent = @event;
            });
            //Act
            await sut.PublishAsync(publishedEvent);
            //Assert
            Assert.Same(publishedEvent, receivedEvent);
        }

        public class Event
        {
        }

        public class AnotherEvent
        {
        }
    }
}
