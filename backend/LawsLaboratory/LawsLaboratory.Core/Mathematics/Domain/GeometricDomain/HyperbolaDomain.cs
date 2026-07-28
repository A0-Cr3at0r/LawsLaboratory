using System.Numerics;

namespace LawsLaboratory.Core.Mathematics.Domain.GeometricDomain;

public sealed class HyperbolaDomain : IDomain<Vector2>
{
    private readonly Vector2 _focus1;
    private readonly Vector2 _focus2;
    private readonly double _distanceDifference;


    public HyperbolaDomain(
        Vector2 focus1,
        Vector2 focus2,
        double distanceDifference)
    {
        if (distanceDifference <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(distanceDifference));

        if (distanceDifference >=
            Vector2.Distance(focus1, focus2))
        {
            throw new ArgumentException(
                "The distance difference must be smaller than the distance between foci.");
        }


        _focus1 = focus1;
        _focus2 = focus2;
        _distanceDifference = distanceDifference;
    }


    public bool Contains(Vector2 point)
    {
        double difference =
            Math.Abs(
                Vector2.Distance(point, _focus1)
                -
                Vector2.Distance(point, _focus2)
            );

        return difference <= _distanceDifference;
    }
}