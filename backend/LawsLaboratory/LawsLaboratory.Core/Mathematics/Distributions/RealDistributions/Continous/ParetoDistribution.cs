using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;

public sealed class ParetoDistribution : IDistribution<double>
{
    private readonly double _min;
    private readonly double _shape;

    private readonly IRandomGenerator _random;


    public ParetoDistribution(
        double min,
        double shape,
        IRandomGenerator random)
    {
        if (min <= 0)
            throw new ArgumentOutOfRangeException(nameof(min));

        if (shape <= 0)
            throw new ArgumentOutOfRangeException(nameof(shape));

        _min = min;
        _shape = shape;

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

        return _min /
               Math.Pow(
                   u,
                   1.0 / _shape);
    }
}