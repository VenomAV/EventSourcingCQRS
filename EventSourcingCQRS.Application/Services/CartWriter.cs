using System.Collections.Generic;
using System.Threading.Tasks;
using EventSourcingCQRS.Domain.CartModule;
using EventSourcingCQRS.Domain.Core;
using EventSourcingCQRS.Domain.CustomerModule;
using EventSourcingCQRS.Domain.Persistence;
using EventSourcingCQRS.Domain.ProductModule;
using EventSourcingCQRS.Domain.PubSub;

namespace EventSourcingCQRS.Application.Services
{
    public class CartWriter : ICartWriter
    {
        private readonly IRepository<Cart, CartId> cartRepository;
        private readonly ITransientDomainEventSubscriber subscriber;
        private readonly IEnumerable<IDomainEventHandler<CartId, CartCreatedEvent>> cartCreatedEventHandlers;
        private readonly IEnumerable<IDomainEventHandler<CartId, ProductAddedEvent>> productAddedEventHandlers;
        private readonly IEnumerable<IDomainEventHandler<CartId, ProductQuantityChangedEvent>> productQuantityChangedEventHandlers;

        public CartWriter(IRepository<Cart, CartId> cartRepository,
            ITransientDomainEventSubscriber subscriber,
            IEnumerable<IDomainEventHandler<CartId, CartCreatedEvent>> cartCreatedEventHandlers,
            IEnumerable<IDomainEventHandler<CartId, ProductAddedEvent>> productAddedEventHandlers,
            IEnumerable<IDomainEventHandler<CartId, ProductQuantityChangedEvent>> productQuantityChangedEventHandlers)
        {
            this.cartRepository = cartRepository;
            this.subscriber = subscriber;
            this.cartCreatedEventHandlers = cartCreatedEventHandlers;
            this.productAddedEventHandlers = productAddedEventHandlers;
            this.productQuantityChangedEventHandlers = productQuantityChangedEventHandlers;
        }

        public async Task AddProductAsync(string cartId, string productId, int quantity)
        {
            var cart = await cartRepository.GetByIdAsync(new CartId(cartId));

            subscriber.Subscribe<ProductAddedEvent>(async @event => await HandleAsync(productAddedEventHandlers, @event));
            cart.AddProduct(new ProductId(productId), quantity);
            await cartRepository.SaveAsync(cart);
        }

        public async Task ChangeProductQuantityAsync(string cartId, string productId, int quantity)
        {
            var cart = await cartRepository.GetByIdAsync(new CartId(cartId));

            subscriber.Subscribe<ProductQuantityChangedEvent>(async @event => await HandleAsync(productQuantityChangedEventHandlers, @event));
            cart.ChangeProductQuantity(new ProductId(productId), quantity);
            await cartRepository.SaveAsync(cart);
        }

        public async Task CreateAsync(string customerId)
        {
            var cart = new Cart(CartId.NewCartId(), new CustomerId(customerId));

            subscriber.Subscribe<CartCreatedEvent>(async @event => await HandleAsync(cartCreatedEventHandlers, @event));
            await cartRepository.SaveAsync(cart);
        }

        public async Task HandleAsync<T>(IEnumerable<IDomainEventHandler<CartId, T>> handlers, T @event)
            where T : IDomainEvent<CartId>
        {
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}
