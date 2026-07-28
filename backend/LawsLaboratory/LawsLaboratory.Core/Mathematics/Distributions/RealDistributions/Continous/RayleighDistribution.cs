using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;

public sealed class RayleighDistribution : IDistribution<double>
{
    private readonly double _scale;
    private readonly IRandomGenerator _random;


    public RayleighDistribution(
        double scale,
        IRandomGenerator random)
    {
        if (scale <= 0)
            throw new ArgumentOutOfRangeException(nameof(scale));

        _scale = scale;
        _random = random;
    }


    public double Generate()
    {
        double u;

        do
        {
            u = _random.NextDouble();
        }
        while (u <= 0);


        return _scale *
               Math.Sqrt(
                   -2 * Math.Log(u));
    }
}