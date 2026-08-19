// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / DiscreteDistributions
//
// BernoulliDistribution.cs
//
// Represents a Bernoulli distribution with success probability p.
//
// Each generated value is either 0 or 1. A value of 1 is produced with
// probability p and a value of 0 otherwise.
//
// Requires 0 <= p <= 1.
// -----------------------------------------------------------------------------
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.DiscreteDistributions;

public sealed class BernoulliDistribution : IDistribution<int>
{
    private readonly double _probability;

    private readonly IRandomGenerator _random;


    public BernoulliDistribution(
        double probability,
        IRandomGenerator random)
    {
        if (probability < 0 || probability > 1)
            throw new ArgumentOutOfRangeException(nameof(probability));

        _probability = probability;
        _random = random;
    }


    public int Generate()
    {
        return _random.NextDouble() < _probability ? 1 : 0;
    }
}