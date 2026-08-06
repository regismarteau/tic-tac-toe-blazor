using Microsoft.AspNetCore.Components;
using RMediator.Abstractions;

namespace Web;

public class DispatcherComponent : ComponentBase, IDisposable
{
    private bool disposedValue;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (RefreshOnDomainEventListened)
        {
            Listeners.Components.Add(this);
        }
    }

    [Inject]
    private IDispatchCommand CommandDispatcher { get; set; } = default!;
    [Inject]
    private IDispatchQuery QueryDispatcher { get; set; } = default!;
    [Inject]
    private DomainEventComponentListeners Listeners { get; set; } = default!;

    private bool RefreshOnDomainEventListened => GetType().GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRefreshComponentOn<>));



    protected Task Dispatch(ICommand command) => CommandDispatcher.Dispatch(command);
    protected Task<TResponse> Dispatch<TResponse>(ICommand<TResponse> command) => CommandDispatcher.Dispatch(command);
    protected Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query) => QueryDispatcher.Dispatch(query);

    protected async Task StateHasChangedAsync()
    {
        await InvokeAsync(StateHasChanged);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing && RefreshOnDomainEventListened)
            {
                Listeners.Components.Remove(this);
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

public interface IRefreshComponentOn<in TDomainEvent> where TDomainEvent : IDomainEvent
{
    Task Refresh(TDomainEvent @event);
}

public class ListenToAllDomainEvents(DomainEventComponentListeners componentListeners) : IListenToDomainEvent<IDomainEvent>
{
    public async Task Listen(IDomainEvent @event, CancellationToken cancellationToken)
    {
        foreach (var tutu in componentListeners.Components.SelectMany(c => c.GetType().GetInterfaces().Where(i =>
            i.IsGenericType &&
            i.GetGenericTypeDefinition() == typeof(IRefreshComponentOn<>) &&
            i.GenericTypeArguments.First().IsAssignableFrom(@event.GetType()))
        .Select(i => new { Component = c, Interface = i })))
        {
            await (Task)tutu.Interface.GetMethod(nameof(IRefreshComponentOn<>.Refresh))!.Invoke(tutu.Component, [@event])!;
        }
    }
}

public class DomainEventComponentListeners
{
    public readonly IList<DispatcherComponent> Components = [];
}