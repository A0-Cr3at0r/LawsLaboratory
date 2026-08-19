// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions
//
// MixtureDistribution.cs
//
// Generates values by selecting one of several distributions according to
// their relative weights.
// -----------------------------------------------------------------------------
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions;

public sealed class MixtureDistribution<T> : IDistribution<T>
{
    private readonly IReadOnlyList<IDistribution<T>> _distributions;
    private readonly double[] _cumulativeWeights;
    private readonly IRandomGenerator _random;

    public MixtureDistribution(
        IReadOnlyList<IDistribution<T>> distributions,
        IReadOnlyList<double> weights,
        IRandomGenerator random)
    {
        if (distributions.Count == 0)
            throw new ArgumentException("At least one distribution must be provided.");

        if (distributions.Count != weights.Count)
            throw new ArgumentException("The number of distributions must match the number of weights.");

        _distributions = distributions;
        _random = random;
        _cumulativeWeights = BuildCumulativeWeights(weights);
    }

    public T Generate()
    {
        double u = _random.NextDouble();

        for (int i = 0; i < _cumulativeWeights.Length; i++)
        {
            if (u < _cumulativeWeights[i])
                return _distributions[i].Generate();
        }

        return _distributions[^1].Generate();
    }

    private static double[] BuildCumulativeWeights(
        IReadOnlyList<double> weights)
    {
        double total = weights.Sum();

        if (total <= 0)
            throw new ArgumentOutOfRangeException(nameof(weights));

        double[] cumulative = new double[weights.Count];

        double sum = 0;

        for (int i = 0; i < weights.Count; i++)
        {
            if (weights[i] < 0)
                throw new ArgumentOutOfRangeException(nameof(weights));

            sum += weights[i] / total;
            cumulative[i] = sum;
        }

        cumulative[^1] = 1;

        return cumulative;
    }
}