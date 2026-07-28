using LawsLaboratory.Core.Mathematics.Distributions;
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.DiscreteDistributions;

public sealed class ZipfDistribution : IDistribution<int>
{
    private readonly double[] _cdf;

    private readonly int _size;

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


        _size = size;
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