using System.Numerics;

namespace LawsLaboratory.Core.Mathematics.Distributions.SpatialDistribution;

public sealed class RadialDistribution : IDistribution<Vector2>
{
    private readonly IDistribution<double> _radiusDistribution;
    private readonly IDistribution<double> _angleDistribution;

    public RadialDistribution(
        IDistribution<double> radiusDistribution,
        IDistribution<double> angleDistribution)
    {
        _radiusDistribution = radiusDistribution;
        _angleDistribution = angleDistribution;
    }

    public Vector2 Generate()
    {
        double radius = _radiusDistribution.Generate();
        double angle = _angleDistribution.Generate();

        return new Vector2(
            (float)(radius * Math.Cos(angle)),
            (float)(radius * Math.Sin(angle)));
    }
}