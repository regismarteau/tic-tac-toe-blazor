using RMediator.Abstractions;
using Web.Dispatchers;

namespace Web.Extensions;

public static class ComponentListeningtoDomainEventExtensions
{
    extension(object component)
    {
        public bool IsComponentListeningToDomainEvent()
            => component.GetType().GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IComponentListeningTo<>));
    }
}
