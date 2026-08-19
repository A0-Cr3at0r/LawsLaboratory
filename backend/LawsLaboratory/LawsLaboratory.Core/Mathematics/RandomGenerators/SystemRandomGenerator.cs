// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / RandomGenerators
//
// SystemRandomGenerator.cs
//
// Implements IRandomGenerator using the .NET System.Random pseudo-random
// number generator.
//
// An optional seed can be supplied to produce a reproducible sequence of
// pseudo-random values.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Core.Mathematics.RandomGenerators;

public sealed class SystemRandomGenerator : IRandomGenerator
{
    private readonly Random _random;

    public SystemRandomGenerator()
    {
        _random = new Random();
    }

    public SystemRandomGenerator(int seed)
    {
        _random = new Random(seed);
    }


    public double NextDouble()
    {
        return _random.NextDouble();
    }
}