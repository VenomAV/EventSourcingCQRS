using EventSourcingCQRS.Domain.Core;
using System;

namespace EventSourcingCQRS.Domain.ProductModule
{
    public class ProductId : IAggregateId
    {
        private const string IdAsStringPrefix = "Product-";

        public Guid Id { get; private set; }

        private ProductId(Guid id)
        {
            Id = id;
        }

        public ProductId(string id)
        {
            Id = Guid.Parse(id.StartsWith(IdAsStringPrefix) ? id.Substring(IdAsStringPrefix.Length) : id);
        }

        public override string ToString()
        {
            return IdAsString();
        }

        public override bool Equals(object obj)
        {
            return obj is ProductId && Equals(Id, ((ProductId)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static ProductId NewProductId()
        {
            return new ProductId(Guid.NewGuid());
        }

        public string IdAsString()
        {
            return $"{IdAsStringPrefix}{Id.ToString()}";
        }

        public static bool operator !=(ProductId left, ProductId right)
        {
            return !(left == right);
        }

        public static bool operator ==(ProductId left, ProductId right)
        {
            return Equals(left?.Id, right?.Id);
        }
    }
}
