using System;
using System.Collections.Generic;

namespace EventSourcingCQRS.Domain.Core
{
    public abstract class DomainEventBase<TAggregateId> : IDomainEvent<TAggregateId>, IEquatable<DomainEventBase<TAggregateId>>
    {
        protected DomainEventBase()
        {
            EventId = Guid.NewGuid();
        }

        protected DomainEventBase(TAggregateId aggregateId) : this()
        {
            AggregateId = aggregateId;
        }

        protected DomainEventBase(TAggregateId aggregateId, long aggregateVersion) : this(aggregateId)
        {
            AggregateVersion = aggregateVersion;
        }

        public Guid EventId { get; private set; }

        public TAggregateId AggregateId { get; private set; }

        public long AggregateVersion { get; private set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as DomainEventBase<TAggregateId>);
        }

        public bool Equals(DomainEventBase<TAggregateId> other)
        {
            return other != null &&
                   EventId.Equals(other.EventId);
        }

        public override int GetHashCode()
        {
            return 290933282 + EqualityComparer<Guid>.Default.GetHashCode(EventId);
        }

        public abstract IDomainEvent<TAggregateId> WithAggregate(TAggregateId aggregateId, long aggregateVersion);
    }
}
