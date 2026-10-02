using Domain.Gameplay;
using Queries;

namespace Web.Extensions;

public static class CellMapper
{
    extension(CellDto cell)
    {
        public Cell ToDomain() => Enum.Parse<Cell>(cell.ToString());
    }
}
