using EventSourcingCQRS.Domain.Core;
using System;
using EventSourcingCQRS.Domain.CustomerModule;
using EventSourcingCQRS.Domain.ProductModule;
using System.Collections.Generic;
using System.Linq;

namespace EventSourcingCQRS.Domain.CartModule
{
    public class Cart : AggregateBase<CartId>
    {
        public const int ProductQuantityThreshold = 50;

        //Needed for persistence purposes
        private Cart()
        {
            Items = new List<CartItem>();
        }

        private CustomerId CustomerId { get; set; }

        private List<CartItem> Items { get; set; }

        public Cart(CartId cartId, CustomerId customerId) : this()
        {
            if (cartId == null)
            {
                throw new ArgumentNullException(nameof(cartId));
            }
            if (customerId == null)
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            RaiseEvent(new CartCreatedEvent(cartId, customerId));
        }

        public void AddProduct(ProductId productId, int quantity)
        {
            if (productId == null)
            {
                throw new ArgumentNullException(nameof(productId));
            }
            if (ContainsProduct(productId))
            {
                throw new CartException($"Product {productId} already added");
            }
            CheckQuantity(productId, quantity);
            RaiseEvent(new ProductAddedEvent(productId, quantity));
        }

        public void ChangeProductQuantity(ProductId productId, int quantity)
        {
            if (!ContainsProduct(productId))
            {
                throw new CartException($"Product {productId} not found");
            }
            CheckQuantity(productId, quantity);
            RaiseEvent(new ProductQuantityChangedEvent(productId, GetCartItemByProduct(productId).Quantity, quantity));
        }

        public override string ToString()
        {
            return $"{{ Id: \"{Id}\", CustomerId: \"{CustomerId.IdAsString()}\", Items: [{string.Join(", ", Items.Select(x => x.ToString()))}] }}";
        }

        internal void Apply(CartCreatedEvent ev)
        {
            Id = ev.AggregateId;
            CustomerId = ev.CustomerId;
        }

        internal void Apply(ProductAddedEvent ev)
        {
            Items.Add(new CartItem(ev.ProductId, ev.Quantity));
        }

        internal void Apply(ProductQuantityChangedEvent ev)
        {
            var existingItem = Items.Single(x => x.ProductId == ev.ProductId);

            Items.Remove(existingItem);
            Items.Add(existingItem.WithQuantity(ev.NewQuantity));
        }

        private bool ContainsProduct(ProductId productId)
        {
            return Items.Any(x => x.ProductId == productId);
        }

        private CartItem GetCartItemByProduct(ProductId productId)
        {
            return Items.Single(x => x.ProductId == productId);
        }

        private static void CheckQuantity(ProductId productId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
            }
            if (quantity > ProductQuantityThreshold)
            {
                throw new CartException($"Quantity for product {productId} must be less than or equal to {ProductQuantityThreshold}");
            }
        }
    }
}
