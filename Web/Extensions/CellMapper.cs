using Domain.ValueObjects;
using Queries;

namespace Web.Extensions;

public static class CellMapper
{
    extension(CellDto cell)
    {
        public Cell ToDomain()
        {
            return Enum.Parse<Cell>(cell.ToString());
        }
    }
}
