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