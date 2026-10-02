using Microsoft.AspNetCore.Components;
using RMediator.Abstractions;
using Web.Extensions;

namespace Web.Dispatchers;

public class DispatcherComponent : ComponentBase, IDisposable
{
    private bool disposedValue;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (this.IsComponentListeningToDomainEvent())
        {
            Listeners.AddListener(this);
        }
    }

    [Inject]
    private IDispatchCommand CommandDispatcher { get; set; } = default!;
    [Inject]
    private IDispatchQuery QueryDispatcher { get; set; } = default!;
    [Inject]
    private DomainEventComponentListeners Listeners { get; set; } = default!;

    protected Task Dispatch(ICommand command, CancellationToken cancellationToken = default) => CommandDispatcher.Dispatch(command, cancellationToken);
    protected Task<TResponse> Dispatch<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default) => CommandDispatcher.Dispatch(command, cancellationToken);
    protected Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default) => QueryDispatcher.Dispatch(query, cancellationToken);

    protected async Task StateHasChangedAsync() => await InvokeAsync(StateHasChanged);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing && this.IsComponentListeningToDomainEvent())
            {
                Listeners.RemoveListener(this);
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
