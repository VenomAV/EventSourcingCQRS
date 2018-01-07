using System.Threading.Tasks;

namespace EventSourcingCQRS.Domain.PubSub
{
    public interface ITransientDomainEventPublisher
    {
        Task PublishAsync<T>(T publishedEvent);
    }
}