using EventSourcingCQRS.Domain.Core;
using System.Collections.Generic;

namespace EventSourcingCQRS.Domain.Persistence
{
    internal interface IEventSourcingAggregate<TAggregateId>
    {
        long Version { get; }
        void ApplyEvent(IDomainEvent<TAggregateId> @event, long version);
        IEnumerable<IDomainEvent<TAggregateId>> GetUncommittedEvents();
        void ClearUncommittedEvents();
    }
}
