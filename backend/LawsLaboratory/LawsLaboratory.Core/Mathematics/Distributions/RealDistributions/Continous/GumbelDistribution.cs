// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / RealDistributions
//
// GumbelDistribution.cs
//
// Represents the Gumbel distribution with the specified location and scale
// parameters.
//
// Samples are generated using inverse transform sampling:
//
//     X = location - scale * ln(-ln(U))
//
// where U is uniformly distributed on (0, 1).
//
// Requires scale > 0.
// -----------------------------------------------------------------------------
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;

public sealed class GumbelDistribution : IDistribution<double>
{
    private readonly double _location;
    private readonly double _scale;

    private readonly IRandomGenerator _random;


    public GumbelDistribution(
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


        return _location -
               _scale *
               Math.Log(
                   -Math.Log(u));
    }
}