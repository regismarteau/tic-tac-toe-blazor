using Bunit;
using Queries;
using Web.Components;

namespace AcceptanceTests.AssertionModels;

public record MarkAssertion(CellDto Cell, SymbolDto? Symbol)
{
    public static MarkAssertion From(IRenderedComponent<CellComponent> cellComponent) => new(cellComponent.Instance.Cell, cellComponent.Instance.Symbol);
}

