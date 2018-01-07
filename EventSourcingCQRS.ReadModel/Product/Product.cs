using EventSourcingCQRS.ReadModel.Common;

namespace EventSourcingCQRS.ReadModel.Product
{
    public class Product : IReadEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
