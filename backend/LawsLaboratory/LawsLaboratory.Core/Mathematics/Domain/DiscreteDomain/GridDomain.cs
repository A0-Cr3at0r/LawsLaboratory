using LawsLaboratory.Core.SpatialModel;

namespace LawsLaboratory.Core.Mathematics.Domain.DiscreteDomain;

public sealed class GridDomain : IDomain<Position>
{
    private readonly int _width;
    private readonly int _height;

    public GridDomain(int width, int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width));

        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height));

        _width = width;
        _height = height;
    }

    public bool Contains(Position position)
    {
        return position.X >= 0 &&
               position.X < _width &&
               position.Y >= 0 &&
               position.Y < _height;
    }
}