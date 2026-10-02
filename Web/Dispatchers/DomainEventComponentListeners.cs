using Microsoft.AspNetCore.Components;
using RMediator.Abstractions;

namespace Web.Dispatchers;

public class DomainEventComponentListeners
{
    private readonly List<ComponentBase> components = [];
    private readonly Lock gate = new();

    public void AddListener(ComponentBase component)
    {
        lock (gate)
        {
            components.Add(component);
        }
    }

    public void RemoveListener(ComponentBase component)
    {
        lock (gate)
        {
            components.Remove(component);
        }
    }

    public IReadOnlyCollection<DomainEventComponentListener> GetAllComponentsListeningTo(IDomainEvent @event)
    {
        ComponentBase[] snapshot;
        lock (gate)
        {
            snapshot = [.. components];
        }

        return [.. snapshot.SelectMany(c => c.GetType().GetInterfaces()
            .Where(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IComponentListeningTo<>) &&
                i.GenericTypeArguments[0].IsAssignableFrom(@event.GetType()))
            .Select(i => new DomainEventComponentListener(c, i)))];
    }
}

public record DomainEventComponentListener(ComponentBase Instance, Type InterfaceType);
