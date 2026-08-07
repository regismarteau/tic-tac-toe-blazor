using AcceptanceTests.Extensions;
using Bunit;
using Domain.ValueObjects;
using FluentAssertions;
using Queries;
using Reqnroll;
using Web.Components;

namespace AcceptanceTests.Steps;

[Binding]
public class GameSteps(ScenarioContext context) : BaseSteps(context)
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
        cells
            .Where(cell => cell.Instance.Symbol is not null)
            .Select(cell => new MarkDto(cell.Instance.Symbol!.Value, cell.Instance.Cell))
            .Should().BeEquivalentTo(ToMarks(table));
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

    private static List<MarkDto> ToMarks(DataTable table)
    {
        return table.Header
            .Concat(table.Rows.SelectMany(row => row.Values))
            .Select((cellContent, index) => new { CellContent = cellContent, Index = index })
            .Where(cell => !string.IsNullOrWhiteSpace(cell.CellContent))
            .Select(cell => new MarkDto(
                Symbol: cell.CellContent.ToLowerInvariant() == "x" ? SymbolDto.Cross : SymbolDto.Nought,
                Cell: (CellDto)cell.Index))
            .ToList();
    }
}
