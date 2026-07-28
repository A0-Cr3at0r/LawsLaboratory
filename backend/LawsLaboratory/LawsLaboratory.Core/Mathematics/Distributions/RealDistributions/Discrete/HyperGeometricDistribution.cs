using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.DiscreteDistributions;

/// <summary>
/// Generates values following a hypergeometric distribution.
/// 
/// The hypergeometric distribution models sampling without replacement
/// from a finite population.
/// 
/// The implementation uses:
/// - direct sequential sampling for small samples;
/// - binomial approximation for negligible sampling fractions;
/// - inverse cumulative distribution sampling for general cases.
/// 
/// </summary>
public sealed class HypergeometricDistribution : IDistribution<int>
{
    private readonly int _populationSize;
    private readonly int _successPopulation;
    private readonly int _sampleSize;

    private readonly IRandomGenerator _random;


    public HypergeometricDistribution(
        int populationSize,
        int successPopulation,
        int sampleSize,
        IRandomGenerator random)
    {
        if (populationSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(populationSize));

        if (successPopulation < 0 ||
            successPopulation > populationSize)
            throw new ArgumentOutOfRangeException(nameof(successPopulation));

        if (sampleSize < 0 ||
            sampleSize > populationSize)
            throw new ArgumentOutOfRangeException(nameof(sampleSize));


        _populationSize = populationSize;
        _successPopulation = successPopulation;
        _sampleSize = sampleSize;

        _random = random;
    }


    public int Generate()
    {
        if (_sampleSize == 0)
            return 0;


        double samplingRatio =
            (double)_sampleSize / _populationSize;


        if (_sampleSize < 64)
            return DirectSampling();


        if (samplingRatio < 0.01)
            return BinomialApproximation();


        return ExactSampling();
    }


    private int DirectSampling()
    {
        int population = _populationSize;
        int success = _successPopulation;

        int result = 0;


        for (int i = 0; i < _sampleSize; i++)
        {
            double probability =
                (double)success / population;


            if (_random.NextDouble() < probability)
            {
                result++;
                success--;
            }

            population--;
        }


        return result;
    }


    private int BinomialApproximation()
    {
        double probability =
            (double)_successPopulation /
            _populationSize;


        return new BinomialDistribution(
            _sampleSize,
            probability,
            _random)
            .Generate();
    }


    private int ExactSampling()
    {
        int min =
            Math.Max(
                0,
                _sampleSize -
                (_populationSize - _successPopulation));

        int max =
            Math.Min(
                _sampleSize,
                _successPopulation);


        double u = _random.NextDouble();

        double probability =
            HypergeometricProbability(min);

        double cumulative = probability;


        for (int k = min; k <= max; k++)
        {
            if (u <= cumulative)
                return k;


            probability *=
                ((double)(_successPopulation - k) /
                 (k + 1)) *
                ((double)(_sampleSize - k) /
                 (_populationSize -
                  _successPopulation -
                  _sampleSize +
                  k + 1));


            cumulative += probability;
        }


        return max;
    }


    private double HypergeometricProbability(int k)
    {
        double numerator =
            Combination(_successPopulation, k) *
            Combination(
                _populationSize - _successPopulation,
                _sampleSize - k);


        double denominator =
            Combination(
                _populationSize,
                _sampleSize);


        return numerator / denominator;
    }


    private double Combination(int n, int k)
    {
        if (k < 0 || k > n)
            return 0;


        double result = 1;


        for (int i = 1; i <= k; i++)
        {
            result *= (n - k + i);
            result /= i;
        }


        return result;
    }
}