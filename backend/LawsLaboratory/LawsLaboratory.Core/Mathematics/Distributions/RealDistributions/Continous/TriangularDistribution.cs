// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / RealDistributions
//
// TriangularDistribution.cs
//
// Represents a triangular distribution defined by its minimum, mode, and
// maximum values.
//
// Samples are generated using inverse transform sampling on the two
// piecewise branches of the triangular cumulative distribution function.
//
// Requires min < max and min <= mode <= max.
// -----------------------------------------------------------------------------

using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;

public sealed class TriangularDistribution : IDistribution<double>
{
    private readonly double _min;
    private readonly double _mode;
    private readonly double _max;

    private readonly double _pivot;

    private readonly IRandomGenerator _random;

    public TriangularDistribution(
        double min,
        double mode,
        double max,
        IRandomGenerator random)
    {
        if (min >= max)
            throw new ArgumentException("min must be less than max.");

        if (mode < min || mode > max)
            throw new ArgumentOutOfRangeException(nameof(mode));

        _min = min;
        _mode = mode;
        _max = max;

        _pivot = (_mode - _min) / (_max - _min);

        _random = random;
    }

    public double Generate()
    {
        double u = _random.NextDouble();

        if (u < _pivot)
        {
            return _min +
                Math.Sqrt(
                    u *
                    (_mode - _min) *
                    (_max - _min));
        }

        return _max -
            Math.Sqrt(
                (1.0 - u) *
                (_max - _min) *
                (_max - _mode));
    }
}