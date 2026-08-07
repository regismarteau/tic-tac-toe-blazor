using RMediator.Abstractions;

namespace Web.Dispatchers;

public interface IComponentListeningTo<in TDomainEvent> where TDomainEvent : IDomainEvent
{
    Task Listen(TDomainEvent @event);
}
