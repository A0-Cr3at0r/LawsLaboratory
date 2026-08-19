// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / DiscreteDistributions
//
// GeometricDistribution.cs
//
// Represents a geometric distribution describing the number of failures
// before the first success.
//
// The generated value belongs to {0, 1, 2, ...} and uses success probability
// p.
//
// Sampling is performed using inverse transform sampling:
//
//     X = floor(ln(1 - U) / ln(1 - p))
//
// where U is uniformly distributed on (0, 1).
//
// Requires 0 < p <= 1.
// -----------------------------------------------------------------------------
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.DiscreteDistributions;

public sealed class GeometricDistribution : IDistribution<int>
{
    private readonly double _probability;

    private readonly IRandomGenerator _random;


    public GeometricDistribution(
        double probability,
        IRandomGenerator random)
    {
        if (probability <= 0 || probability > 1)
            throw new ArgumentOutOfRangeException(nameof(probability));

        _probability = probability;
        _random = random;
    }


    public int Generate()
    {
        double u;

        do
        {
            u = _random.NextDouble();
        }
        while (u <= 0);


        return (int)(
            Math.Log(1 - u) /
            Math.Log(1 - _probability));
    }
}