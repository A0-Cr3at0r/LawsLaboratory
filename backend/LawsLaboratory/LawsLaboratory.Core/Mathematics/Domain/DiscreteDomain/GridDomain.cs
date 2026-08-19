using LawsLaboratory.Core.SpatialModel.Position;

namespace LawsLaboratory.Core.Mathematics.Domain.DiscreteDomain;
/// <summary>
/// Represents the finite rectangular domain of a two-dimensional grid.
/// Coordinates are valid when they fall within the configured width
/// and height.
/// </summary>
public sealed class GridDomain : IDomain<PlanePosition>
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

    public bool Contains(PlanePosition position)
    {
        return position.X >= 0 &&
               position.X < _width &&
               position.Y >= 0 &&
               position.Y < _height;
    }
}