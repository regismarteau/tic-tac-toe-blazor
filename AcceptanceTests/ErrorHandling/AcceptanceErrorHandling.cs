using Reqnroll;
using RMediator.Abstractions;

namespace AcceptanceTests.ErrorHandling
{
    public class AcceptanceErrorHandling<TRequest, TResponse>(ScenarioContext context) : IHandleMiddleware<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, NextMiddleware<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next(request, cancellationToken);
            }
            catch (Exception ex)
            {
                if (!context.IsAnErrorHandlingScenario())
                {
                    throw;
                }
                context.Set(new AcceptanceError(ex));
                return default!;
            }
        }
    }

    public class AcceptanceErrorHandling<TRequest>(ScenarioContext context) : IHandleMiddleware<TRequest> where TRequest : IRequest
    {
        public async Task Handle(TRequest request, NextMiddleware next, CancellationToken cancellationToken)
        {
            try
            {
                await next(request, cancellationToken);
            }
            catch (Exception ex)
            {
                if (!context.IsAnErrorHandlingScenario())
                {
                    throw;
                }

                context.Set(new AcceptanceError(ex));
            }
        }
    }
}
