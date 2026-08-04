using System.Diagnostics;


namespace LawsLaboratory.Application.Observer.Performance;


internal sealed class PipelinePerformanceMetrics
{
    private readonly Queue<long> _arrivalTimes;

    private readonly long _startTimestamp;

    private long _completedCount;

    private long _totalLatencyTicks;

    private long _maxLatencyTicks;


    public PipelinePerformanceMetrics()
    {
        _arrivalTimes = new Queue<long>();

        _startTimestamp =
            Stopwatch.GetTimestamp();
    }

    public void DataArrived()
    {
        _arrivalTimes.Enqueue(
            Stopwatch.GetTimestamp());
    }

    public void DataCompleted()
    {
        if (_arrivalTimes.Count == 0)
            return;

        long start =
            _arrivalTimes.Dequeue();

        long elapsed =
            Stopwatch.GetTimestamp() - start;

        _completedCount++;
        _totalLatencyTicks += elapsed;

        if (elapsed > _maxLatencyTicks)
        {
            _maxLatencyTicks = elapsed;
        }
    }


    public long CompletedCount =>
        _completedCount;

    public TimeSpan AverageLatency
    {
        get
        {
            if (_completedCount == 0)
                return TimeSpan.Zero;


            return StopwatchTicksToTimeSpan(
                _totalLatencyTicks / _completedCount);
        }
    }

    public TimeSpan MaxLatency =>
        StopwatchTicksToTimeSpan(
            _maxLatencyTicks);

    public double Throughput
    {
        get
        {
            double elapsedSeconds =
                (double)
                (
                    Stopwatch.GetTimestamp()
                    -
                    _startTimestamp
                )
                /
                Stopwatch.Frequency;


            if (elapsedSeconds <= 0)
                return 0;


            return _completedCount / elapsedSeconds;
        }
    }

    private static TimeSpan StopwatchTicksToTimeSpan(
        long ticks)
    {
        return TimeSpan.FromSeconds(
            (double)ticks / Stopwatch.Frequency);
    }
}