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