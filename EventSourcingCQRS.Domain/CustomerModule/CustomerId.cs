using EventSourcingCQRS.Domain.Core;
using System;

namespace EventSourcingCQRS.Domain.CustomerModule
{
    public class CustomerId : IAggregateId
    {
        private const string IdAsStringPrefix = "Customer-";

        public Guid Id { get; private set; }

        private CustomerId(Guid id)
        {
            Id = id;
        }

        public CustomerId(string id)
        {
            Id = Guid.Parse(id.StartsWith(IdAsStringPrefix) ? id.Substring(IdAsStringPrefix.Length) : id);
        }

        public override string ToString()
        {
            return IdAsString();
        }

        public override bool Equals(object obj)
        {
            return obj is CustomerId && Equals(Id, ((CustomerId)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static CustomerId NewCustomerId()
        {
            return new CustomerId(Guid.NewGuid());
        }

        public string IdAsString()
        {
            return $"{IdAsStringPrefix}{Id.ToString()}";
        }
    }
}