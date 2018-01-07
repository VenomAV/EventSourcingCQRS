using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventSourcingCQRS.ReadModel.Cart;
using EventSourcingCQRS.ReadModel.Persistence;

namespace EventSourcingCQRS.Application.Services
{
    public class CartReader : ICartReader
    {
        private readonly IReadOnlyRepository<Cart> cartRepository;
        private readonly IReadOnlyRepository<CartItem> cartItemRepository;

        public CartReader(IReadOnlyRepository<Cart> cartRepository, IReadOnlyRepository<CartItem> cartItemRepository)
        {
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
        }

        public async Task<IEnumerable<Cart>> FindAllAsync(Expression<Func<Cart, bool>> predicate)
        {
            return await cartRepository.FindAllAsync(predicate);
        }

        public async Task<Cart> GetByIdAsync(string id)
        {
            return await cartRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<CartItem>> GetItemsOfAsync(string cartId)
        {
            return await cartItemRepository.FindAllAsync(x => x.CartId == cartId);
        }
    }
}
