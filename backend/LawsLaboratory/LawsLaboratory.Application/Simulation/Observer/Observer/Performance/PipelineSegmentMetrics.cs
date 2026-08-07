using System.Diagnostics;

namespace LawsLaboratory.Application.Simulation.Observer.Observer.Performance;

using LawsLaboratory.Application.Observer.Observation;
using LawsLaboratory.Application.Simulation.Observer;
using LawsLaboratory.Application.Simulation.Observer.Observation;

internal readonly record struct FlowPoint(
    ObserverChannel Channel,
    FlowDirection Direction);



internal sealed class PipelineSegmentMetrics
{
    private readonly FlowPoint _source;

    private readonly FlowPoint _destination;


    private readonly Queue<long> _startTimes;


    private long _completedCount;

    private long _totalLatencyTicks;

    private long _maxLatencyTicks;


    public PipelineSegmentMetrics(
        FlowPoint source,
        FlowPoint destination)
    {
        _source = source;
        _destination = destination;

        _startTimes = new Queue<long>();
    }


    public void Notify(
        FlowObservation observation)
    {
        FlowPoint point =
            new(
                observation.Channel,
                observation.Direction);


        if (point == _source)
        {
            DataArrived();
            return;
        }


        if (point == _destination)
        {
            DataLeft();
        }
    }

    private void DataArrived()
    {
        _startTimes.Enqueue(
            Stopwatch.GetTimestamp());
    }

    private void DataLeft()
    {
        if (_startTimes.Count == 0)
            return;


        long start =
            _startTimes.Dequeue();


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


    private static TimeSpan StopwatchTicksToTimeSpan(
        long ticks)
    {
        return TimeSpan.FromSeconds(
            (double)ticks / Stopwatch.Frequency);
    }
}