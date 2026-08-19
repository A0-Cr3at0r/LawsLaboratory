// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Domain / GeometricDomain
//
// IntervalDomain.cs
//
// Represents a closed interval on the real number line.
//
// A value belongs to the domain when it lies between the minimum and maximum
// bounds, inclusively.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Core.Mathematics.Domain.GeometricDomain;

public sealed class IntervalDomain : IDomain<double>
{
    private readonly double _min;
    private readonly double _max;

    public IntervalDomain(double min, double max)
    {
        if (min > max)
            throw new ArgumentException("Minimum cannot be greater than maximum.");

        _min = min;
        _max = max;
    }

    public bool Contains(double value)
    {
        return value >= _min && value <= _max;
    }
}