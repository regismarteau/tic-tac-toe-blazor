using AcceptanceTests.AssertionModels;
using AcceptanceTests.Extensions;
using Bunit;
using Domain.ValueObjects;
using FluentAssertions;
using Queries;
using Reqnroll;
using Web.Components;

namespace AcceptanceTests.Steps;

[Binding]
public partial class GameSteps(ScenarioContext context) : BaseSteps(context)
{
    [Given("a game started")]
    [When("I start a new game")]
    public async Task WhenIStartANewGame()
    {
        await Page.FindByDataTest("start-button").ClickAsync();
    }

    [When("^I play on (.+?) cell$")]
    public async Task WhenIPlayOnTopLeftCell(Cell cell)
    {
        await Page.FindByDataTest($"cell-{cell}").ClickAsync();
        await Context.WaitForSideEffects();
    }

    [Then("the game looks like")]
    public async Task ThenTheGameLooksLike(DataTable table)
    {
        var cells = Page.FindComponents<CellComponent>();
        cells.Select(MarkAssertion.From)
            .Should()
            .BeEquivalentTo(ToMarks(table));
    }

    [Then("the game ends in a draw")]
    public async Task ThenTheGameEndsInADraw(DataTable table)
    {
        await ThenTheGameLooksLike(table);
        Page.FindByDataTest("draw-modal").Should().NotBeNull();
    }

    [Then("^the game has been won by the computer$")]
    public async Task ThenTheGameHasBeenWonBy(DataTable table)
    {
        await ThenTheGameLooksLike(table);
        Page.FindByDataTest("you-loose-modal").Should().NotBeNull();
    }

    private static List<MarkAssertion> ToMarks(DataTable table)
    {
        return [.. table.Header
            .Concat(table.Rows.SelectMany(row => row.Values))
            .Select((cellContent, index) => new { CellContent = cellContent, Index = index })
            .Select(cell => new MarkAssertion(
                Cell: (CellDto)cell.Index,
                Symbol: string.IsNullOrWhiteSpace(cell.CellContent) ?
                    null :
                    cell.CellContent.Equals("x", StringComparison.InvariantCultureIgnoreCase) ?
                        SymbolDto.Cross :
                        SymbolDto.Nought))];
    }
}
