using AcceptanceTests.ErrorHandling;
using FluentAssertions;
using Reqnroll;

namespace AcceptanceTests.Steps;

public class ErrorSteps(ScenarioContext context) : BaseSteps(context)
{
    private readonly ScenarioContext context = context;

    [Then("^an (.+) error occured$")]
    public void ThenAnCellAlreadyMarkedExceptionErrorOccured(string error)
    {
        var acceptanceError = context.Get<AcceptanceError>();
        acceptanceError.Exception.Message.Should().Be(error);
    }
}
