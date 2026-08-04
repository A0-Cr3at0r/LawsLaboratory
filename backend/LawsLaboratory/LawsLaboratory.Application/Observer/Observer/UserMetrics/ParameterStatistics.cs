namespace LawsLaboratory.Application.Observer.Observer.UserMetrics;


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

        _spatialStatistics = new RunningStatistics();

        _temporalStatistics = new RunningStatistics();
    }

    public void ObserveCell(double? value)
    {
        _receivedCells++;

        if (value is double validValue)
        {
            _spatialStatistics.Push(validValue);
        }



        if (_receivedCells == _cellCount)
        {
            CompleteIteration();
        }
    }

    private void CompleteIteration()
    {
        _temporalStatistics.Push(
            _spatialStatistics.Mean);

        ResetSpatialIteration();
    }

    private void ResetSpatialIteration()
    {
        _receivedCells = 0;

        _spatialStatistics.Reset();
    }

    public bool CompleteIterationIfReady()
    {
        if (_receivedCells == 0)
        {
            CompleteIteration();
            return true;
        }
        return false;
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