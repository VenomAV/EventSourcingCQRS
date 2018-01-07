using EventSourcingCQRS.Domain.Core;
using EventSourcingCQRS.Domain.ProductModule;

namespace EventSourcingCQRS.Domain.CartModule
{
    public class ProductAddedEvent : DomainEventBase<CartId>
    {
        ProductAddedEvent()
        {
        }

        internal ProductAddedEvent(ProductId productId, int quantity) : base()
        {
            ProductId = productId;
            Quantity = quantity;
        }

        internal ProductAddedEvent(CartId aggregateId, long aggregateVersion, ProductId productId, int quantity) : base(aggregateId, aggregateVersion)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public ProductId ProductId { get; private set; }

        public int Quantity { get; private set; }

        public override IDomainEvent<CartId> WithAggregate(CartId aggregateId, long aggregateVersion)
        {
            return new ProductAddedEvent(aggregateId, aggregateVersion, ProductId, Quantity);
        }
    }
}
