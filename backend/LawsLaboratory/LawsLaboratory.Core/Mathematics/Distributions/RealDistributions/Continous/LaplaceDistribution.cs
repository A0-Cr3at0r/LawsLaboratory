using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;

public sealed class LaplaceDistribution : IDistribution<double>
{
    private readonly double _mean;
    private readonly double _scale;

    private readonly IRandomGenerator _random;


    public LaplaceDistribution(
        double mean,
        double scale,
        IRandomGenerator random)
    {
        if (scale <= 0)
            throw new ArgumentOutOfRangeException(nameof(scale));

        _mean = mean;
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


        if (u < 0.5)
        {
            return _mean +
                   _scale *
                   Math.Log(2 * u);
        }

        return _mean -
               _scale *
               Math.Log(2 * (1 - u));
    }
}