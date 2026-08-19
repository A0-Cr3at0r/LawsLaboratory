// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Domain / GeometricDomain
//
// BoxDomain.cs
//
// Represents a rectangular region of the plane defined by independent
// intervals on the X and Y axes.
// -----------------------------------------------------------------------------
using System.Numerics;

namespace LawsLaboratory.Core.Mathematics.Domain.GeometricDomain;

public sealed class BoxDomain : IDomain<Vector2>
{
    private readonly IntervalDomain _xDomain;
    private readonly IntervalDomain _yDomain;

    public BoxDomain(
        double minX,
        double maxX,
        double minY,
        double maxY)
    {
        _xDomain = new IntervalDomain(minX, maxX);
        _yDomain = new IntervalDomain(minY, maxY);
    }

    public bool Contains(Vector2 value)
    {
        return _xDomain.Contains(value.X) &&
               _yDomain.Contains(value.Y);
    }
}