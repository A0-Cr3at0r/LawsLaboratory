// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Observer / Observer / UserMetrics
//
// ParameterStatistics.cs
//
// Accumulates statistics for one parameter during simulation iterations.
//
// Spatial statistics are collected from valid cell values of the current
// iteration. Dead / NaN observations are excluded from numerical statistics.
//
// When all cells of an iteration have been received, the spatial mean is
// recorded as one temporal observation and the spatial accumulator is reset.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.Observer.Observer.UserMetrics;

public sealed class ParameterStatistics
{
    private readonly int _cellCount;

    private int _receivedCells;

    private readonly RunningStatistics _spatialStatistics;

    private readonly RunningStatistics _temporalStatistics;


    public ParameterStatistics(int cellCount)
    {
        if (cellCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(cellCount));

        _cellCount = cellCount;

        _spatialStatistics =
            new RunningStatistics();

        _temporalStatistics =
            new RunningStatistics();
    }


    /// <summary>
    /// Observes one cell.
    ///
    /// Returns true when this observation completes the current iteration.
    /// Dead / NaN values are received and counted, but do not contribute to
    /// spatial statistics.
    /// </summary>
    public bool ObserveCell(double? value)
    {
        _receivedCells++;

        if (value is double validValue &&
            !double.IsNaN(validValue))
        {
            _spatialStatistics.Push(validValue);
        }

        if (_receivedCells != _cellCount)
            return false;

        CompleteIteration();

        return true;
    }


    private void CompleteIteration()
    {
        // An iteration is recorded only when at least one valid value exists.
        if (_spatialStatistics.Count > 0)
        {
            _temporalStatistics.Push(
                _spatialStatistics.Mean);
        }

        ResetSpatialIteration();
    }


    private void ResetSpatialIteration()
    {
        _receivedCells = 0;

        _spatialStatistics.Reset();
    }


    public ParameterMetricSnapshot CreateSnapshot(
        ushort parameterId)
    {
        return new ParameterMetricSnapshot(
            parameterId,
            _spatialStatistics.CreateSnapshot(),
            _temporalStatistics.CreateSnapshot());
    }


    public double CurrentTemporalMean =>
        _temporalStatistics.Mean;
}