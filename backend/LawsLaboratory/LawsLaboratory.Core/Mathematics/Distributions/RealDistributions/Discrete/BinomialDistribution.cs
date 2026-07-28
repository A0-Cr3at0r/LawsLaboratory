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