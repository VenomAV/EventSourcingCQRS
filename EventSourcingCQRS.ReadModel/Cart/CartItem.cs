using EventSourcingCQRS.ReadModel.Common;

namespace EventSourcingCQRS.ReadModel.Cart
{
    public class CartItem : IReadEntity
    {
        public string Id { get; private set; }

        public string CartId { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public static CartItem CreateFor(string cartId, string productId)
        {
            return new CartItem
            {
                Id = IdFor(cartId, productId),
                CartId = cartId,
                ProductId = productId
            };
        }

        public static string IdFor(string cartId, string productId)
        {
            return $"{productId}@{cartId}";
        }
    }
}
