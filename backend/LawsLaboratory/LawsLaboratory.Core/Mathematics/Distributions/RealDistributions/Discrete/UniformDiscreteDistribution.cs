// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / DiscreteDistributions
//
// DiscreteUniformDistribution.cs
//
// Represents a discrete uniform distribution over the integer interval
// [min, max].
//
// Each integer in the interval has equal probability of being generated.
//
// Requires min <= max.
// -----------------------------------------------------------------------------
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.DiscreteDistributions;

public sealed class DiscreteUniformDistribution : IDistribution<int>
{
    private readonly int _min;
    private readonly int _size;

    private readonly IRandomGenerator _random;


    public DiscreteUniformDistribution(
        int min,
        int max,
        IRandomGenerator random)
    {
        if (max < min)
            throw new ArgumentException("max must be greater than or equal to min.");

        _min = min;
        _size = max - min + 1;

        _random = random;
    }


    public int Generate()
    {
        return _min +
               (int)(_random.NextDouble() * _size);
    }
}