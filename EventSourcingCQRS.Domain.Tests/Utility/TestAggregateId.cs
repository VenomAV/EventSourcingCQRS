using EventSourcingCQRS.Domain.Core;

namespace EventSourcingCQRS.Domain.Tests.Utility
{
    public class TestAggregateId : IAggregateId
    {
        public string IdAsString()
        {
            return "";
        }
    }
}
