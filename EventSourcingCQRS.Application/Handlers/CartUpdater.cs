using EventSourcingCQRS.Domain.CartModule;
using EventSourcingCQRS.ReadModel.Customer;
using EventSourcingCQRS.ReadModel.Persistence;
using EventSourcingCQRS.ReadModel.Product;
using CartReadModel = EventSourcingCQRS.ReadModel.Cart.Cart;
using CartItemReadModel = EventSourcingCQRS.ReadModel.Cart.CartItem;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingCQRS.Application.Services;

namespace EventSourcingCQRS.Application.Handlers
{
    public class CartUpdater : IDomainEventHandler<CartId, CartCreatedEvent>,
        IDomainEventHandler<CartId, ProductAddedEvent>, 
        IDomainEventHandler<CartId, ProductQuantityChangedEvent>
    {
        private readonly IReadOnlyRepository<Customer> customerRepository;
        private readonly IReadOnlyRepository<Product> productRepository;
        private readonly IRepository<CartReadModel> cartRepository;
        private readonly IRepository<CartItemReadModel> cartItemRepository;

        public CartUpdater(IReadOnlyRepository<Customer> customerRepository,
            IReadOnlyRepository<Product> productRepository, IRepository<CartReadModel> cartRepository,
            IRepository<CartItemReadModel> cartItemRepository)
        {
            this.customerRepository = customerRepository;
            this.productRepository = productRepository;
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
        }

        public async Task HandleAsync(CartCreatedEvent @event)
        {
            var customer = await customerRepository.GetByIdAsync(@event.CustomerId.IdAsString());

            await cartRepository.InsertAsync(new CartReadModel
            {
                    Id = @event.AggregateId.IdAsString(),
                    CustomerId = customer.Id,
                    CustomerName = customer.Name,
                    TotalItems = 0
                });
        }

        public async Task HandleAsync(ProductAddedEvent @event)
        {
            var product = await productRepository.GetByIdAsync(@event.ProductId.IdAsString());
            var cart = await cartRepository.GetByIdAsync(@event.AggregateId.IdAsString());
            var cartItem = CartItemReadModel.CreateFor(@event.AggregateId.IdAsString(), @event.ProductId.IdAsString());

            cartItem.ProductName = product.Name;
            cartItem.Quantity = @event.Quantity;
            cart.TotalItems += @event.Quantity;
            await cartRepository.UpdateAsync(cart);
            await cartItemRepository.InsertAsync(cartItem);
        }

        public async Task HandleAsync(ProductQuantityChangedEvent @event)
        {
            var cartItemId = CartItemReadModel.IdFor(@event.AggregateId.IdAsString(), @event.ProductId.IdAsString());
            var cartItem = (await cartItemRepository
                .FindAllAsync(x => x.Id == cartItemId))
                .Single();
            var cart = await cartRepository.GetByIdAsync(@event.AggregateId.IdAsString());

            cart.TotalItems += @event.NewQuantity - @event.OldQuantity;
            cartItem.Quantity = @event.NewQuantity;

            await cartRepository.UpdateAsync(cart);
            await cartItemRepository.UpdateAsync(cartItem);
        }
    }
}
