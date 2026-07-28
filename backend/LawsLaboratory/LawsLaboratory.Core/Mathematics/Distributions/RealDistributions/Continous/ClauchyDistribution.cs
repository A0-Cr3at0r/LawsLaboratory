using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;

public sealed class CauchyDistribution : IDistribution<double>
{
    private readonly double _location;
    private readonly double _scale;

    private readonly IRandomGenerator _random;


    public CauchyDistribution(
        double location,
        double scale,
        IRandomGenerator random)
    {
        if (scale <= 0)
            throw new ArgumentOutOfRangeException(nameof(scale));

        _location = location;
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
        while (u <= 0 || u >= 1);


        return _location +
               _scale *
               Math.Tan(
                   Math.PI *
                   (u - 0.5));
    }
}