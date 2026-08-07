using AcceptanceTests.Extensions;
using Bunit;
using Reqnroll;
using Web.Pages;

namespace AcceptanceTests.Steps;

[Binding]
public class BaseSteps(ScenarioContext context)
{
    private IRenderedComponent<Home>? page;
    protected ScenarioContext Context { get; } = context;

    protected IRenderedComponent<Home> Page
    {
        get
        {
            page ??= Context.GetTestServices().Render<Home>();
            return page;
        }
    }
}
