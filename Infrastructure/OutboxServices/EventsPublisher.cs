using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMediator.Abstractions;

namespace Infrastructure.OutboxServices;

public class EventsPublisher(FirstOrDefaultEventPublisher eventPublisher, DomainEventToPublishAwaiter awaiter, ILogger<EventsPublisher> logger)
{
    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await PublishNextEvent(stoppingToken);
        }
    }

    private async Task PublishNextEvent(CancellationToken stoppingToken)
    {
        try
        {
            await awaiter.WaitForADomainEvent(stoppingToken);
            await eventPublisher.PublishFirstOrDefaultEvent(stoppingToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            logger.LogError(exception, "Publication of an outbox event failed");
        }
    }
}

public class FirstOrDefaultEventPublisher(TicTacToeDbContext dbContext, DbContextSaveChanges changes, IPublishDomainEvent publisher)
{
    public async Task<bool> PublishFirstOrDefaultEvent(CancellationToken stoppingToken)
    {
        var eventEntity = await dbContext.Outbox.FirstOrDefaultAsync(stoppingToken);
        if (eventEntity is null)
        {
            return false;
        }
        var domainEvent = eventEntity.Deserialize();
        await publisher.Publish(domainEvent, stoppingToken);
        dbContext.Outbox.Remove(eventEntity);
        await changes.SaveAsync(stoppingToken);

        return true;
    }
}
