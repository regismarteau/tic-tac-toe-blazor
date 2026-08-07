using RMediator.Abstractions;

namespace Web.Dispatchers;

public class ListenToAllDomainEvents(DomainEventComponentListeners componentListeners) : IListenToDomainEvent<IDomainEvent>
{
    public async Task Listen(IDomainEvent @event, CancellationToken cancellationToken)
    {
        foreach (var component in componentListeners.GetAllComponentsListeningTo(@event))
        {
            await (Task)component.InterfaceType.GetMethod(nameof(IComponentListeningTo<>.Listen))!.Invoke(component.Instance, [@event])!;
        }
    }
}
