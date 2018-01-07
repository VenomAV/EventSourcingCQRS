using EventSourcingCQRS.Domain.Core;
using System.Threading.Tasks;

namespace EventSourcingCQRS.Application.Services
{
    public interface IDomainEventHandler<TAggregateId, TEvent>
        where TEvent: IDomainEvent<TAggregateId>
    {
        Task HandleAsync(TEvent @event);
    }
}
