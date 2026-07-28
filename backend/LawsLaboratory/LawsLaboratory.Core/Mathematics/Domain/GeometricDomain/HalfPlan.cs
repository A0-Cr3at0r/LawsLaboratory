using System.Numerics;

namespace LawsLaboratory.Core.Mathematics.Domain.GeometricDomain;

public sealed class HalfPlaneDomain : IDomain<Vector2>
{
    private readonly double _a;
    private readonly double _b;
    private readonly double _c;

    public HalfPlaneDomain(
        double a,
        double b,
        double c)
    {
        if (a == 0 && b == 0)
            throw new ArgumentException(
                "A and B cannot both be zero.");

        _a = a;
        _b = b;
        _c = c;
    }

    public bool Contains(Vector2 point)
    {
        return _a * point.X +
               _b * point.Y +
               _c <= 0;
    }
}