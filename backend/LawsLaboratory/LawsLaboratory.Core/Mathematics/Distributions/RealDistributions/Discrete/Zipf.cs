// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / DiscreteDistributions
//
// ZipfDistribution.cs
//
// Represents a finite Zipf distribution over the integers 1 through size.
//
// The probability of generating rank k is proportional to:
//
//     1 / k^exponent
//
// The cumulative distribution function is precomputed during construction
// and binary search is used to generate samples.
//
// Requires size > 0 and exponent > 0.
// -----------------------------------------------------------------------------
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.DiscreteDistributions;

public sealed class ZipfDistribution : IDistribution<int>
{
    private readonly double[] _cdf;


    private readonly IRandomGenerator _random;


    public ZipfDistribution(
        int size,
        double exponent,
        IRandomGenerator random)
    {
        if (size <= 0)
            throw new ArgumentOutOfRangeException(nameof(size));

        if (exponent <= 0)
            throw new ArgumentOutOfRangeException(nameof(exponent));

        _random = random;

        _cdf = BuildCDF(size, exponent);
    }


    public int Generate()
    {
        double u = _random.NextDouble();

        int index =
            Array.BinarySearch(_cdf, u);


        if (index < 0)
            index = ~index;


        return index + 1;
    }


    private static double[] BuildCDF(
        int size,
        double exponent)
    {
        double[] cdf = new double[size];

        double normalization = 0;


        for (int i = 1; i <= size; i++)
        {
            normalization +=
                1 / Math.Pow(i, exponent);
        }


        double cumulative = 0;


        for (int i = 1; i <= size; i++)
        {
            cumulative +=
                (1 / Math.Pow(i, exponent))
                / normalization;

            cdf[i - 1] = cumulative;
        }


        return cdf;
    }
}