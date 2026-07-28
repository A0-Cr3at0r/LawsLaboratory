using System.Numerics;

namespace LawsLaboratory.Core.Mathematics.Domain.GeometricDomain;

public sealed class EllipseDomain : IDomain<Vector2>
{
    private readonly Vector2 _focus1;
    private readonly Vector2 _focus2;
    private readonly double _majorAxis;

    public EllipseDomain(
        Vector2 focus1,
        Vector2 focus2,
        double majorAxis)
    {
        if (majorAxis <= 0)
            throw new ArgumentOutOfRangeException(nameof(majorAxis));

        double distance =
            Vector2.Distance(focus1, focus2);

        if (majorAxis * 2 < distance)
            throw new ArgumentException(
                "The major axis must be greater than the distance between foci.");

        _focus1 = focus1;
        _focus2 = focus2;
        _majorAxis = majorAxis;
    }


    public bool Contains(Vector2 point)
    {
        double distance =
            Vector2.Distance(point, _focus1) +
            Vector2.Distance(point, _focus2);

        return distance <= 2 * _majorAxis;
    }
}