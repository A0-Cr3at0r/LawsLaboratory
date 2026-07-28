using LawsLaboratory.Core.Mathematics.Distributions;
using LawsLaboratory.Core.Mathematics.RandomGenerators;

/// <summary>
/// Generates normally distributed random values using
/// the Box-Muller transform.
///
/// Reference:
/// Box, G. E. P. and Muller, M. E. (1958).
/// A Note on the Generation of Random Normal Deviates.
/// </summary>
public sealed class NormalDistribution : IDistribution<double>
{
    private readonly double _mean;
    private readonly double _standardDeviation;
    private readonly IRandomGenerator _random;

    public NormalDistribution(
        double mean,
        double standardDeviation,
        IRandomGenerator random)
    {
        if (standardDeviation <= 0)
            throw new ArgumentOutOfRangeException(nameof(standardDeviation));

        _mean = mean;
        _standardDeviation = standardDeviation;
        _random = random;
    }


    public double Generate()
    {
        double u0;

        do {
            u0 = _random.NextDouble();
        } while (u0 <= 0);

        double u1 = _random.NextDouble();

        double r = Math.Sqrt(-2 * Math.Log(u0));
        double theta = 2 * u1 * Math.PI;

        double z = r * Math.Cos(theta);

        return _mean + z * _standardDeviation;
    }
}