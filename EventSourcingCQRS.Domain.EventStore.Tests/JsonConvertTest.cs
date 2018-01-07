using EventSourcingCQRS.Domain.CartModule;
using EventSourcingCQRS.Domain.CustomerModule;
using Newtonsoft.Json;
using System;
using Xunit;

namespace EventSourcingCQRS.Domain.EventStore.Tests
{
    [Trait("Type", "Unit")]
    public class JsonConvertTest
    {
        [Fact]
        public void DeserializeEvent()
        {
            var json = "{\"CustomerId\":{\"Id\":\"9154d569-0bf3-4c37-b588-ed49765c45dc\"},\"EventId\":\"3907f8bd-6b4b-4825-8a7c-b63800e7f28f\",\"AggregateId\":{\"Id\":\"23934182-ea29-4c99-8b4a-845c4b152153\"},\"AggregateVersion\":-1}";

            var contractResolver = new PrivateSetterContractResolver();
            JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = contractResolver };
            var type = Type.GetType("EventSourcingCQRS.Domain.CartModule.CartCreatedEvent, EventSourcingCQRS.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            var @event = JsonConvert.DeserializeObject(json, type, settings);

            Assert.IsType<CartCreatedEvent>(@event);
            Assert.Equal(new CustomerId("9154d569-0bf3-4c37-b588-ed49765c45dc"), ((CartCreatedEvent)@event).CustomerId);
            Assert.Equal(new CartId("23934182-ea29-4c99-8b4a-845c4b152153"), ((CartCreatedEvent)@event).AggregateId);
            Assert.Equal(new Guid("3907f8bd-6b4b-4825-8a7c-b63800e7f28f"), ((CartCreatedEvent)@event).EventId);
            Assert.Equal(-1, ((CartCreatedEvent)@event).AggregateVersion);
        }
    }
}
