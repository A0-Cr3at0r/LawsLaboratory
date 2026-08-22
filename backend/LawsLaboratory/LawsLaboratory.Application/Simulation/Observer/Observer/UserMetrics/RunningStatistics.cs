// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Observer / Observer / UserMetrics
//
// RunningStatistics.cs
//
// Maintains numerical statistics incrementally without storing individual
// observations.
//
// Mean and variance are updated using Welford's online algorithm. Minimum and
// maximum are tracked alongside the accumulated count, allowing snapshots to be
// produced without retaining the underlying data.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Application.Simulation.Observer.Observer.UserMetrics;

public sealed class RunningStatistics
{
    private long _count;

    private double _mean;

    private double _m2;

    private double _minimum = double.PositiveInfinity;

    private double _maximum = double.NegativeInfinity;


    public long Count => _count;

    public double Mean =>
        _count > 0
            ? _mean
            : double.NaN;

    public double Variance =>
        _count > 0
            ? _m2 / _count
            : double.NaN;

    public double StandardDeviation =>
        _count > 0
            ? Math.Sqrt(Variance)
            : double.NaN;

    public double Minimum =>
        _count > 0
            ? _minimum
            : double.NaN;

    public double Maximum =>
        _count > 0
            ? _maximum
            : double.NaN;


    public void Push(double value)
    {
        _count++;

        double delta =
            value - _mean;

        _mean +=
            delta / _count;

        double delta2 =
            value - _mean;

        _m2 +=
            delta * delta2;


        if (value < _minimum)
            _minimum = value;

        if (value > _maximum)
            _maximum = value;
    }


    public void Reset()
    {
        _count = 0;
        _mean = 0;
        _m2 = 0;
        _minimum = double.PositiveInfinity;
        _maximum = double.NegativeInfinity;
    }


    public StatisticsSnapshot CreateSnapshot()
    {
        return new StatisticsSnapshot(
            Count,
            Mean,
            Variance,
            StandardDeviation,
            Minimum,
            Maximum);
    }
}
