using Microsoft.AspNetCore.Components;
using RMediator.Abstractions;

namespace Web.Dispatchers;

public class DomainEventComponentListeners
{
    private readonly IList<ComponentBase> Components = [];

    public void AddListener(ComponentBase component)
    {
        Components.Add(component);
    }

    public void RemoveListener(ComponentBase component)
    {
        Components.Remove(component);
    }

    public IReadOnlyCollection<DomainEventComponentListener> GetAllComponentsListeningTo(IDomainEvent @event)
    {
        return [.. Components.SelectMany(c => c.GetType().GetInterfaces()
        .Where(i =>
            i.IsGenericType &&
            i.GetGenericTypeDefinition() == typeof(IComponentListeningTo<>) &&
            i.GenericTypeArguments.First().IsAssignableFrom(@event.GetType()))
        .Select(i => new DomainEventComponentListener(c, i)))];
    }
}

public record DomainEventComponentListener(ComponentBase Instance, Type InterfaceType);
