using AcceptanceTests.Extensions;
using Bunit;
using Reqnroll;
using Web.Pages;

namespace AcceptanceTests.Steps;

[Binding]
public class BaseSteps(ScenarioContext context)
{
    protected ScenarioContext Context { get; } = context;

    protected IRenderedComponent<Home> Page
    {
        get
        {
            field ??= Context.GetTestServices().Render<Home>();
            return field;
        }
    }
}
