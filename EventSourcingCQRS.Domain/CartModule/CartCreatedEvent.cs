using EventSourcingCQRS.Domain.Core;
using EventSourcingCQRS.Domain.CustomerModule;

namespace EventSourcingCQRS.Domain.CartModule
{
    public class CartCreatedEvent : DomainEventBase<CartId>
    {
        CartCreatedEvent()
        {
        }

        internal CartCreatedEvent(CartId aggregateId, CustomerId customerId) : base(aggregateId)
        {
            CustomerId = customerId;
        }

        private CartCreatedEvent(CartId aggregateId, long aggregateVersion, CustomerId customerId) : base(aggregateId, aggregateVersion)
        {
            CustomerId = customerId;
        }

        public CustomerId CustomerId { get; private set; }

        public override IDomainEvent<CartId> WithAggregate(CartId aggregateId, long aggregateVersion)
        {
            return new CartCreatedEvent(aggregateId, aggregateVersion, CustomerId);
        }
    }
}