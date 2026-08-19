// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / RealDistributions
//
// RayleighDistribution.cs
//
// Represents the Rayleigh distribution with the specified scale parameter.
//
// Samples are generated using inverse transform sampling:
//
//     X = scale * sqrt(-2 * ln(U))
//
// where U is uniformly distributed on (0, 1).
//
// Requires scale > 0.
// -----------------------------------------------------------------------------

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