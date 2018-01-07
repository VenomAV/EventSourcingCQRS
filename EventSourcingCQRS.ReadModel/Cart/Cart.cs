using EventSourcingCQRS.ReadModel.Common;

namespace EventSourcingCQRS.ReadModel.Cart
{
    public class Cart : IReadEntity
    {
        public string Id { get; set; }

        public int TotalItems { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }
    }
}
