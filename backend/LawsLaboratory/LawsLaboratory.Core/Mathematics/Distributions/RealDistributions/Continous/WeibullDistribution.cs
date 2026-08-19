// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / RealDistributions
//
// WeibullDistribution.cs
//
// Represents the Weibull distribution parameterized by shape and scale.
//
// Samples are generated using inverse transform sampling:
//
//     X = scale * (-ln(U))^(1 / shape)
//
// where U is uniformly distributed on (0, 1).
//
// Requires shape > 0 and scale > 0.
// -----------------------------------------------------------------------------
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