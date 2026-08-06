using Database;
using Microsoft.EntityFrameworkCore;
using RMediator.Abstractions;

namespace Infrastructure.OutboxServices;

public class EventsPublisher(FirstOrDefaultEventPublisher eventPublisher)
{
    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!await eventPublisher.PublishFirstOrDefaultEvent(stoppingToken))
            {
                await Task.Delay(10, stoppingToken);
            }
        }
    }
}

public class FirstOrDefaultEventPublisher(TicTacToeDbContext dbContext, IPublishDomainEvent publisher)
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
        await dbContext.SaveChangesAsync(stoppingToken);

        return true;
    }
}