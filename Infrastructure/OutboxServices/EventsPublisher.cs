using Database;
using Microsoft.EntityFrameworkCore;
using RMediator.Abstractions;

namespace Infrastructure.OutboxServices;

public class EventsPublisher(FirstOrDefaultEventPublisher eventPublisher, DomainEventToPublishAwaiter awaiter)
{
    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await awaiter.WaitForADomainEvent(stoppingToken);
            await eventPublisher.PublishFirstOrDefaultEvent(stoppingToken);
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
