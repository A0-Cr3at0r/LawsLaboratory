// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / DiscreteDistributions
//
// BinomialDistribution.cs
//
// Represents a binomial distribution with a fixed number of independent
// Bernoulli trials.
//
// The generated value represents the number of successful trials among n
// trials, where each trial has success probability p.
//
// Sampling is performed by generating the underlying Bernoulli trials.
//
// Requires trials >= 0 and 0 <= p <= 1.
// -----------------------------------------------------------------------------
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.DiscreteDistributions;

public sealed class BinomialDistribution : IDistribution<int>
{
    private readonly int _trials;

    private readonly BernoulliDistribution _bernoulli;


    public BinomialDistribution(
        int trials,
        double probability,
        IRandomGenerator random)
    {
        if (trials < 0)
            throw new ArgumentOutOfRangeException(nameof(trials));

        _trials = trials;

        _bernoulli = new BernoulliDistribution(
            probability,
            random);
    }


    public int Generate()
    {
        int success = 0;

        for (int i = 0; i < _trials; i++)
        {
            success += _bernoulli.Generate();
        }

        return success;
    }
}