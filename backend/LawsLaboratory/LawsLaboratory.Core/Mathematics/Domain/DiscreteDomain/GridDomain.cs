// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Domain / DiscreteDomain
//
// GridDomain.cs
//
// Represents the finite rectangular domain of a two-dimensional discrete
// grid.
//
// A PlanePosition belongs to the domain when its coordinates fall within the
// configured grid width and height. The lower bounds are inclusive and the
// upper bounds are exclusive.
//
// GridDomain operates on the discrete PlanePosition representation and is
// therefore distinct from the continuous geometric domains used during
// spatial initialization.
// -----------------------------------------------------------------------------

using LawsLaboratory.Core.SpatialModel.Position;

namespace LawsLaboratory.Core.Mathematics.Domain.DiscreteDomain;

public sealed class GridDomain : IDomain<PlanePosition>
{
    private readonly int _width;
    private readonly int _height;

    public GridDomain(
        int width,
        int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width));

        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height));

        _width = width;
        _height = height;
    }

    public bool Contains(
        PlanePosition position)
    {
        return position.X >= 0 &&
               position.X < _width &&
               position.Y >= 0 &&
               position.Y < _height;
    }
}
