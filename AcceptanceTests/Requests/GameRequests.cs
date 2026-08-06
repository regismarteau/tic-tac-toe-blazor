using AcceptanceTests.Configuration;
using Domain.ValueObjects;
using Queries;
using Reqnroll;
using UseCases.Commands;

namespace AcceptanceTests.Requests
{
    public class GameRequests(ScenarioContext context)
    {
        public async Task<Guid> Start()
        {
            return await context.Dispatch(new StartAGame());
        }

        public async Task<GameDto> GetGame(Guid gameId)
        {
            return await context.Dispatch(new GetGameState(gameId));
        }

        public async Task Play(Guid gameId, Cell cell)
        {
            await context.Dispatch(new Play(new(gameId), cell));
        }
    }
}
