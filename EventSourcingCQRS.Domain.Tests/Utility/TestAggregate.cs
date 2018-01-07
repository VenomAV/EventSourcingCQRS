using EventSourcingCQRS.Domain.Core;
using System.Collections.Generic;

namespace EventSourcingCQRS.Domain.Tests.Utility
{
    public class TestAggregate : AggregateBase<TestAggregateId>
    {
        public readonly List<IDomainEvent<TestAggregateId>> AppliedEvents = new List<IDomainEvent<TestAggregateId>>();

        private TestAggregate()
        {

        }

        public TestAggregate(params TestDomainEvent[] events)
        {
            foreach (var @event in events)
            {
                RaiseEvent(@event);
            }
        }

        public void Apply(TestDomainEvent @event)
        {
            AppliedEvents.Add(@event);
        }
    }
}
