// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / DiscreteDistributions
//
// NegativeBinomialDistribution.cs
//
// Represents a negative binomial distribution describing the number of
// failures observed before a fixed number of successes is reached.
//
// The distribution is parameterized by the required number of successes and
// the probability of success on each independent trial.
//
// Sampling uses the Gamma-Poisson mixture representation of the negative
// binomial distribution.
//
// Requires:
// - successCount > 0
// - 0 < probability < 1
// -----------------------------------------------------------------------------

using LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.DiscreteDistributions;

public sealed class NegativeBinomialDistribution : IDistribution<int>
{

    private readonly GammaDistribution _gamma;

    private readonly IRandomGenerator _random;

    public NegativeBinomialDistribution(
        int successCount,
        double probability,
        IRandomGenerator random)
    {
        if (successCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(successCount));

        if (probability <= 0 || probability >= 1)
            throw new ArgumentOutOfRangeException(nameof(probability));



        _random = random;

        _gamma = new GammaDistribution(
            successCount,
            (1 - probability) / probability,
            _random);

    }


    public int Generate()
    {
        double lambda = _gamma.Generate();

        return new PoissonDistribution(
            lambda,
            _random)
            .Generate();
    }
}