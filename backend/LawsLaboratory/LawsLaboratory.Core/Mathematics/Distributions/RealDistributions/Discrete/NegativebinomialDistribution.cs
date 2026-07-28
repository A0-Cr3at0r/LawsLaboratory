using LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.DiscreteDistributions;

public sealed class NegativeBinomialDistribution : IDistribution<int>
{
    private readonly int _successCount;
    private readonly double _probability;

    private readonly GammaDistribution _gamma;
    private readonly PoissonDistribution _poisson;


    public NegativeBinomialDistribution(
        int successCount,
        double probability,
        IRandomGenerator random)
    {
        if (successCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(successCount));

        if (probability <= 0 || probability >= 1)
            throw new ArgumentOutOfRangeException(nameof(probability));


        _successCount = successCount;
        _probability = probability;


        _gamma = new GammaDistribution(
            successCount,
            (1 - probability) / probability,
            random);

        _poisson = new PoissonDistribution(
            1,
            random);
    }


    public int Generate()
    {
        double lambda = _gamma.Generate();

        return new PoissonDistribution(
            lambda,
            _poisson.Random)
            .Generate();
    }
}