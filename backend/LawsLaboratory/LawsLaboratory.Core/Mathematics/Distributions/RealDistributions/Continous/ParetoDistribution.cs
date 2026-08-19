// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / RealDistributions
//
// ParetoDistribution.cs
//
// Represents a Pareto distribution with the specified minimum value and
// shape parameter.
//
// Samples are generated using inverse transform sampling:
//
//     X = min / U^(1 / shape)
//
// where U is uniformly distributed on (0, 1).
//
// Requires min > 0 and shape > 0.
// -----------------------------------------------------------------------------
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