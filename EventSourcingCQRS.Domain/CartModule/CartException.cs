using System;

namespace EventSourcingCQRS.Domain.CartModule
{

    [Serializable]
    public class CartException : Exception
    {
        public CartException() { }
        public CartException(string message) : base(message) { }
        public CartException(string message, Exception inner) : base(message, inner) { }
        protected CartException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
