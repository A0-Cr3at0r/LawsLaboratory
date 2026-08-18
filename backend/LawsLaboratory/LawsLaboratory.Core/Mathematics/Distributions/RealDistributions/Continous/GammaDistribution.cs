using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;

/// <summary>
/// Generates Gamma distributed random values using the
/// Marsaglia-Tsang method.
///
/// For shape >= 1, the algorithm is applied directly.
/// For 0 < shape < 1, a Gamma(shape + 1, 1) is first generated,
/// then transformed into Gamma(shape, 1).
///
/// Reference:
/// Marsaglia, G., Tsang, W. W. (2000).
/// A Simple Method for Generating Gamma Variables.
/// ACM Transactions on Mathematical Software,
/// 26(3), 363-372.
/// </summary>
public sealed class GammaDistribution : IDistribution<double>
{
    private readonly double _shape;
    private readonly double _scale;

    private readonly double _d;
    private readonly double _c;

    private readonly IRandomGenerator _random;
    private readonly NormalDistribution _normal;
    private readonly GammaDistribution? _shapePlusOneDistribution;

    public GammaDistribution(
        double shape,
        double scale,
        IRandomGenerator random)
    {
        if (shape <= 0)
            throw new ArgumentOutOfRangeException(nameof(shape));

        if (scale <= 0)
            throw new ArgumentOutOfRangeException(nameof(scale));

        _shape = shape;
        _scale = scale;

        _random = random;
        _normal = new NormalDistribution(0, 1, random);

        if (_shape < 1.0)
        {
            _shapePlusOneDistribution = new GammaDistribution(
                _shape + 1.0,
                1.0,
                random);
        }
        else
        {
            _d = _shape - 1.0 / 3.0;
            _c = 1.0 / Math.Sqrt(9.0 * _d);
        }
    }

    public double Generate()
    {
        if (_shape < 1.0)
        {

            double y = _shapePlusOneDistribution!.Generate();

            double u = _random.NextDouble();

            while (u <= 0.0)
                u = _random.NextDouble();

            return _scale * y * Math.Pow(u, 1.0 / _shape);
        }

        while (true)
        {
            double z = _normal.Generate();

            double v = 1.0 + _c * z;

            if (v <= 0.0)
                continue;

            v *= v * v;

            double u = _random.NextDouble();

            if (QuickTest(z, u))
                return _scale * _d * v;

            if (ExactTest(z, v, u))
                return _scale * _d * v;
        }
    }

    private static bool QuickTest(
        double z,
        double u)
    {
        return u < 1.0 - 0.0331 * Math.Pow(z, 4);
    }

    private bool ExactTest(
        double z,
        double v,
        double u)
    {
        return Math.Log(u)
            < 0.5 * z * z
            + _d * (1.0 - v + Math.Log(v));
    }
}