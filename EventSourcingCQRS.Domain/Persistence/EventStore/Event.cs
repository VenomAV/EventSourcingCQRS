using EventSourcingCQRS.Domain.Core;

namespace EventSourcingCQRS.Domain.Persistence.EventStore
{
    public class Event<TAggregateId>
    {
        public Event(IDomainEvent<TAggregateId> domainEvent, long eventNumber)
        {
            DomainEvent = domainEvent;
            EventNumber = eventNumber;
        }

        public long EventNumber { get; }

        public IDomainEvent<TAggregateId> DomainEvent { get; }
    }
}
