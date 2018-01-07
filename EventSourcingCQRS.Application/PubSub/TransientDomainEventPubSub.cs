using EventSourcingCQRS.Domain.PubSub;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingCQRS.Application.PubSub
{
    public class TransientDomainEventPubSub : IDisposable, ITransientDomainEventSubscriber, ITransientDomainEventPublisher
    {
        private static AsyncLocal<Dictionary<Type, List<object>>> handlers = new AsyncLocal<Dictionary<Type, List<object>>>();

        public Dictionary<Type, List<object>> Handlers
        {
            get => handlers.Value ?? (handlers.Value = new Dictionary<Type, List<object>>());
        }

        public TransientDomainEventPubSub()
        {
        }

        public void Dispose()
        {
            foreach (var handlersOfT in Handlers.Values)
            {
                handlersOfT.Clear();
            }
            Handlers.Clear();
        }

        public void Subscribe<T>(Action<T> handler)
        {
            GetHandlersOf<T>().Add(handler);
        }

        public void Subscribe<T>(Func<T, Task> handler)
        {
            GetHandlersOf<T>().Add(handler);
        }

        public async Task PublishAsync<T>(T publishedEvent)
        {
            foreach (var handler in GetHandlersOf<T>())
            {
                try
                {
                    switch (handler)
                    {
                        case Action<T> action:
                            action(publishedEvent);
                            break;
                        case Func<T, Task> action:
                            await action(publishedEvent);
                            break;
                        default:
                            break;
                    }
                }
                catch
                {
                    //Logging
                }
            }
        }

        private ICollection<object> GetHandlersOf<T>()
        {
            return Handlers.GetValueOrDefault(typeof(T)) ?? (Handlers[typeof(T)] = new List<object>());
        }
    }
}
