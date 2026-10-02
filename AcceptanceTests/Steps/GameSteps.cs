using AcceptanceTests.AssertionModels;
using AcceptanceTests.Extensions;
using Bunit;
using Domain.Gameplay;
using FluentAssertions;
using Queries;
using Reqnroll;
using Web.Components;
using Web.Shared.DataTests;

namespace AcceptanceTests.Steps;

[Binding]
public class GameSteps(ScenarioContext context) : BaseSteps(context)
{
    [Given("a game started")]
    [When("I start a new game")]
    public async Task WhenIStartANewGame() => await Page.FindByDataTest(TicTacToeDataTests.StartButton).ClickAsync();

    [When("^I play on (.+?) cell$")]
    public async Task WhenIPlayOnCell(Cell cell)
    {
        await Page.FindByDataTest(TicTacToeDataTests.Cell.For(cell)).ClickAsync();
        await Context.WaitForSideEffects();
    }

    [When("I retry a new game")]
    public async Task WhenIRetryANewGame() => await Page.FindByDataTest(TicTacToeDataTests.RetryButton).ClickAsync();

    [Then("the game looks like")]
    public void ThenTheGameLooksLike(DataTable table)
    {
        var cells = Page.FindComponents<CellComponent>();
        cells.Select(MarkAssertion.From)
            .Should()
            .BeEquivalentTo(ToMarks(table));
    }

    [Then("the game ends in a draw")]
    public void ThenTheGameEndsInADraw(DataTable table)
    {
        ThenTheGameLooksLike(table);
        Page.FindByDataTest(TicTacToeDataTests.DrawModal).Should().NotBeNull();
    }

    [Then("^the game has been won by the computer$")]
    public void ThenTheGameHasBeenWonByTheComputer(DataTable table)
    {
        ThenTheGameLooksLike(table);
        Page.FindByDataTest(TicTacToeDataTests.YouLooseModal).Should().NotBeNull();
    }

    private static List<MarkAssertion> ToMarks(DataTable table) =>
    [
        .. table.Header
            .Concat(table.Rows.SelectMany(row => row.Values))
            .Select((cellContent, index) => new
            {
                CellContent = cellContent,
                Index = index
            })
            .Select(cell => new MarkAssertion(
                (CellDto)cell.Index,
                string.IsNullOrWhiteSpace(cell.CellContent) ?
                    null :
                    cell.CellContent.Equals("x", StringComparison.InvariantCultureIgnoreCase) ?
                        SymbolDto.Cross :
                        SymbolDto.Nought))
    ];
}
