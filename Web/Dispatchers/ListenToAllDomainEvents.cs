using RMediator.Abstractions;

namespace Web.Dispatchers;

public class ListenToAllDomainEvents(DomainEventComponentListeners componentListeners, ILogger<ListenToAllDomainEvents> logger) : IListenToDomainEvent<IDomainEvent>
{
    public async Task Listen(IDomainEvent @event, CancellationToken cancellationToken)
    {
        foreach (var component in componentListeners.GetAllComponentsListeningTo(@event))
        {
            try
            {
                await (Task)component.InterfaceType.GetMethod(nameof(IComponentListeningTo<>.Listen))!.Invoke(component.Instance, [@event, cancellationToken])!;
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                logger.LogError(e, "Component {Component} failed to handle {Event}", component.Instance.GetType().Name, @event.GetType().Name);
            }
        }
    }
}
