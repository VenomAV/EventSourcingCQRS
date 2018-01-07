using System;

namespace EventSourcingCQRS.Domain.Core
{
    public interface IDomainEvent<TAggregateId>
    {
        /// <summary>
        /// The event identifier
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// The identifier of the aggregate which has generated the event
        /// </summary>
        TAggregateId AggregateId { get;  }

        /// <summary>
        /// The version of the aggregate when the event has been generated
        /// </summary>
        long AggregateVersion { get; }
    }
}
