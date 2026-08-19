// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Domain / GeometricDomain
//
// ParabolaDomain.cs
//
// Represents one of the two regions defined by a parabola, using a focus and
// a directrix. The selected side is determined by comparing the distance from
// a point to the focus with its distance to the directrix.
// -----------------------------------------------------------------------------

using System.Numerics;

namespace LawsLaboratory.Core.Mathematics.Domain.GeometricDomain;

public sealed class ParabolaDomain : IDomain<Vector2>
{
    private readonly Vector2 _focus;

    private readonly double _a;
    private readonly double _b;
    private readonly double _c;

    private readonly bool _includeCloserSide;


    public ParabolaDomain(
        Vector2 focus,
        double a,
        double b,
        double c,
        bool includeCloserSide = true)
    {
        if (a == 0 && b == 0)
            throw new ArgumentException(
                "The directrix requires a valid normal vector.");

        _focus = focus;

        _a = a;
        _b = b;
        _c = c;

        _includeCloserSide = includeCloserSide;
    }


    public bool Contains(Vector2 point)
    {
        double focusDistance =
            Vector2.Distance(point, _focus);


        double lineDistance =
            Math.Abs(
                _a * point.X +
                _b * point.Y +
                _c)
            /
            Math.Sqrt(
                _a * _a +
                _b * _b);


        bool closer =
            focusDistance <= lineDistance;


        return _includeCloserSide
            ? closer
            : !closer;
    }
}