using EventSourcingCQRS.Domain.Core;
using System;

namespace EventSourcingCQRS.Domain.CartModule
{
    public class CartId : IAggregateId
    {
        private const string IdAsStringPrefix = "Cart-";

        public Guid Id { get; private set; }

        private CartId(Guid id)
        {
            Id = id;
        }

        public CartId(string id)
        {
            Id = Guid.Parse(id.StartsWith(IdAsStringPrefix) ? id.Substring(IdAsStringPrefix.Length) : id);
        }

        public override string ToString()
        {
            return IdAsString();
        }

        public override bool Equals(object obj)
        {
            return obj is CartId && Equals(Id, ((CartId)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static CartId NewCartId()
        {
            return new CartId(Guid.NewGuid());
        }

        public string IdAsString()
        {
            return $"{IdAsStringPrefix}{Id.ToString()}";
        }
    }
}
