using EventSourcingCQRS.Domain.Core;
using EventSourcingCQRS.Domain.ProductModule;

namespace EventSourcingCQRS.Domain.CartModule
{
    public class ProductQuantityChangedEvent : DomainEventBase<CartId>
    {
        ProductQuantityChangedEvent()
        {
        }

        internal ProductQuantityChangedEvent(ProductId productId, int oldQuantity, int newQuantity) : base()
        {
            ProductId = productId;
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
        }

        private ProductQuantityChangedEvent(CartId aggregateId, long aggregateVersion, ProductId productId, 
            int oldQuantity, int newQuantity) : base(aggregateId, aggregateVersion)
        {
            ProductId = productId;
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
        }

        public ProductId ProductId { get; private set; }

        public int OldQuantity { get; private set; }

        public int NewQuantity { get; private set; }

        public override IDomainEvent<CartId> WithAggregate(CartId aggregateId, long aggregateVersion)
        {
            return new ProductQuantityChangedEvent(aggregateId, aggregateVersion,ProductId, OldQuantity, NewQuantity);
        }
    }
}
