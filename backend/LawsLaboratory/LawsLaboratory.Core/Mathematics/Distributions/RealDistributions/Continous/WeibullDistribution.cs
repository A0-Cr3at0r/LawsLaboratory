using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;

public sealed class WeibullDistribution : IDistribution<double>
{
    private readonly double _shape;
    private readonly double _scale;

    private readonly IRandomGenerator _random;


    public WeibullDistribution(
        double shape,
        double scale,
        IRandomGenerator random)
    {
        if (shape <= 0)
            throw new ArgumentOutOfRangeException(nameof(shape));

        if (scale <= 0)
            throw new ArgumentOutOfRangeException(nameof(scale));

        _shape = shape;
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
               Math.Pow(
                   -Math.Log(u),
                   1.0 / _shape);
    }
}