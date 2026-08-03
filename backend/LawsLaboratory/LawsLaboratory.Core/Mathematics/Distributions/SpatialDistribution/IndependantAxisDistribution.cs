using System.Numerics;


namespace LawsLaboratory.Core.Mathematics.Distributions.SpatialDistribution;
public sealed class IndependentAxisDistribution
    : IDistribution<Vector2>
{
    private readonly IDistribution<double> _xDistribution;
    private readonly IDistribution<double> _yDistribution;

    public IndependentAxisDistribution(
        IDistribution<double> xDistribution,
        IDistribution<double> yDistribution)
    {
        _xDistribution = xDistribution;
        _yDistribution = yDistribution;
    }

    public Vector2 Generate()
    {
        return new Vector2((float)_xDistribution.Generate(), 
                            (float)_yDistribution.Generate());
    }
}