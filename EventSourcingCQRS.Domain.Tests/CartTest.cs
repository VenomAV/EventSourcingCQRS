using EventSourcingCQRS.Domain.CartModule;
using EventSourcingCQRS.Domain.CustomerModule;
using EventSourcingCQRS.Domain.ProductModule;
using EventSourcingCQRS.Domain.Tests.Utility;
using Xunit;

namespace EventSourcingCQRS.Domain.Tests
{
    [Trait("Type", "Unit")]
    public class CartTest : GenericAggregateBaseTest<Cart, CartId>
    {
        private static readonly CustomerId DefaultCustomerId = CustomerId.NewCustomerId();
        private static readonly CartId DefaultCartId = CartId.NewCartId();
        private static readonly ProductId DefaultProductId = ProductId.NewProductId();

        [Fact]
        public void GivenNoCartExistsWhenCreateOneThenCartCreatedEvent()
        {
            var cart = new Cart(DefaultCartId, DefaultCustomerId);

            AssertSingleUncommittedEvent<CartCreatedEvent>(cart, @event =>
            {
                Assert.Equal(DefaultCartId, @event.AggregateId);
                Assert.Equal(DefaultCustomerId, @event.CustomerId);
            });
        }

        [Fact]
        public void GivenACartWhenAddAProductThenProductAddedEvent()
        {
            var cart = new Cart(DefaultCartId, DefaultCustomerId);
            ClearUncommittedEvents(cart);

            cart.AddProduct(DefaultProductId, 2);

            AssertSingleUncommittedEvent<ProductAddedEvent>(cart, @event =>
            {
                Assert.Equal(DefaultProductId, @event.ProductId);
                Assert.Equal(2, @event.Quantity);
                Assert.Equal(DefaultCartId, @event.AggregateId);
                Assert.Equal(0, @event.AggregateVersion);
            });
        }

        [Fact]
        public void GivenACartWithAProductWhenAddingTheSameProductThenThrowsCartException()
        {
            var cart = new Cart(DefaultCartId, DefaultCustomerId);

            cart.AddProduct(DefaultProductId, 2);
            ClearUncommittedEvents(cart);

            Assert.Throws<CartException>(() => { cart.AddProduct(DefaultProductId, 1); });
            Assert.Empty(GetUncommittedEventsOf(cart));
        }

        [Fact]
        public void GivenACartWithAProductWhenChangingQuantityThenProductQuantityChangedEvent()
        {
            var cart = new Cart(DefaultCartId, DefaultCustomerId);

            cart.AddProduct(DefaultProductId, 2);
            ClearUncommittedEvents(cart);
            cart.ChangeProductQuantity(DefaultProductId, 3);
            AssertSingleUncommittedEvent<ProductQuantityChangedEvent>(cart, @event =>
            {
                Assert.Equal(DefaultProductId, @event.ProductId);
                Assert.Equal(2, @event.OldQuantity);
                Assert.Equal(3, @event.NewQuantity);
                Assert.Equal(DefaultCartId, @event.AggregateId);
                Assert.Equal(1, @event.AggregateVersion);
            });
        }

        [Fact]
        public void GivenACartWhenChangingQuantityOfAMissingProductThenThrowsCartException()
        {
            var cart = new Cart(DefaultCartId, DefaultCustomerId);

            Assert.Throws<CartException>(() => { cart.ChangeProductQuantity(DefaultProductId, 3); });
        }

        [Fact]
        public void GivenAnEmptyCarWhenAddingAProductAndRequestedQuantityIsGreaterThanProductThresholdThenThrowsCartException()
        {
            var cart = new Cart(DefaultCartId, DefaultCustomerId);

            Assert.Throws<CartException>(() => { cart.AddProduct(DefaultProductId, 51); });
        }

        [Fact]
        public void GivenACartWithAProductWhenRequestedQuantityIsGreaterThanProductThresholdThenThrowsCartException()
        {
            var cart = new Cart(DefaultCartId, DefaultCustomerId);

            cart.AddProduct(DefaultProductId, 1);
            Assert.Throws<CartException>(() => { cart.ChangeProductQuantity(DefaultProductId, 51); });
        }
    }
}
