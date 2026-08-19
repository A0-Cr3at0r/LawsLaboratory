// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / DiscreteDistributions
//
// MultinomialDistribution.cs
//
// Represents a multinomial distribution describing the counts obtained when
// performing a fixed number of independent trials across multiple categories.
//
// Each category has an associated probability and the probabilities must sum
// to 1.
//
// Sampling is performed by decomposing the multinomial distribution into a
// sequence of conditional binomial distributions.
//
// Requires:
// - trials >= 0
// - at least one category
// - probabilities >= 0
// - sum of probabilities = 1
// -----------------------------------------------------------------------------
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.DiscreteDistributions;

public sealed class MultinomialDistribution : IDistribution<int[]>
{
    private readonly int _trials;
    private readonly double[] _probabilities;

    private readonly IRandomGenerator _random;


    public MultinomialDistribution(
        int trials,
        double[] probabilities,
        IRandomGenerator random)
    {
        if (trials < 0)
            throw new ArgumentOutOfRangeException(nameof(trials));

        if (probabilities.Length == 0)
            throw new ArgumentException("At least one probability is required.");

        if (Math.Abs(probabilities.Sum() - 1) > 1e-10)
            throw new ArgumentException("Probabilities must sum to 1.");

        foreach (var probability in probabilities)
        {
            if (probability < 0)
                throw new ArgumentOutOfRangeException(nameof(probability));
        }

        _trials = trials;
        _probabilities = (double[])probabilities.Clone();
        _random = random;
    }


    public int[] Generate()
    {
        int[] result = new int[_probabilities.Length];

        int remainingTrials = _trials;
        double remainingProbability = 1;


        for (int i = 0; i < _probabilities.Length - 1; i++)
        {
            double conditionalProbability =
                _probabilities[i] / remainingProbability;


            BinomialDistribution binomial =
                new(
                    remainingTrials,
                    conditionalProbability,
                    _random);


            result[i] = binomial.Generate();


            remainingTrials -= result[i];
            remainingProbability -= _probabilities[i];
        }


        result[^1] = remainingTrials;

        return result;
    }
}