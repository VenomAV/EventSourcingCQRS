using EventSourcingCQRS.Domain.ProductModule;

namespace EventSourcingCQRS.Domain.CartModule
{
    public class CartItem
    {
        public CartItem(ProductId productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public ProductId ProductId { get; }

        public int Quantity { get; }

        public override bool Equals(object obj)
        {
            var castedObj = obj as CartItem;
            return castedObj != null && Equals(castedObj.ProductId, ProductId) 
                && Equals(castedObj.Quantity, Quantity);
        }

        public override int GetHashCode()
        {
            var hashCode = 76325633;
            hashCode = hashCode * -1521134295 + ProductId.GetHashCode();
            hashCode = hashCode * -1521134295 + Quantity.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{{ ProductId: \"{ProductId}\", Quantity: {Quantity} }}";
        }

        public CartItem WithQuantity(int quantity)
        {
            return new CartItem(ProductId, quantity);
        }
    }
}
