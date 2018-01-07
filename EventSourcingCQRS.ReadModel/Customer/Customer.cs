using EventSourcingCQRS.ReadModel.Common;

namespace EventSourcingCQRS.ReadModel.Customer
{
    public class Customer : IReadEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
