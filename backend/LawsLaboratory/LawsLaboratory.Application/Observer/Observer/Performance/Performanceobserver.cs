using LawsLaboratory.Application.Observer.Observation;

namespace LawsLaboratory.Application.Observer.Performance;

public sealed class PerformanceObserver :
    IDataObserver<FlowObservation>
{

    private readonly PipelineSegmentMetrics[] _segments;

    private readonly PipelinePerformanceMetrics _global;



    public PerformanceObserver()
    {
        _segments =
        [
            // RequestBuffer residence
            new PipelineSegmentMetrics(
                new(
                    ObserverChannel.RequestBuffer,
                    FlowDirection.Enter),

                new(
                    ObserverChannel.RequestBuffer,
                    FlowDirection.Exit)),


            // Request -> GatewayEntry
            new PipelineSegmentMetrics(
                new(
                    ObserverChannel.RequestBuffer,
                    FlowDirection.Exit),

                new(
                    ObserverChannel.GatewayEntry,
                    FlowDirection.Enter)),


            // GatewayEntry residence
            new PipelineSegmentMetrics(
                new(
                    ObserverChannel.GatewayEntry,
                    FlowDirection.Enter),

                new(
                    ObserverChannel.GatewayEntry,
                    FlowDirection.Exit)),


            // Gateway processing
            new PipelineSegmentMetrics(
                new(
                    ObserverChannel.GatewayExit,
                    FlowDirection.Enter),

                new(
                    ObserverChannel.GatewayExit,
                    FlowDirection.Exit)),


            // GatewayExit -> ResultBuffer
            new PipelineSegmentMetrics(
                new(
                    ObserverChannel.GatewayExit,
                    FlowDirection.Exit),

                new(
                    ObserverChannel.ResultBuffer,
                    FlowDirection.Enter)),


            // ResultBuffer residence
            new PipelineSegmentMetrics(
                new(
                    ObserverChannel.ResultBuffer,
                    FlowDirection.Enter),

                new(
                    ObserverChannel.ResultBuffer,
                    FlowDirection.Exit))
        ];


        _global =
            new PipelinePerformanceMetrics();
    }


    public void Notify(
        FlowObservation observation)
    {
        foreach (var segment in _segments)
        {
            segment.Notify(observation);
        }


        if (observation.Channel ==
            ObserverChannel.RequestBuffer
            &&
            observation.Direction ==
            FlowDirection.Enter)
        {
            _global.DataArrived();
        }


        if (observation.Channel ==
            ObserverChannel.ResultBuffer
            &&
            observation.Direction ==
            FlowDirection.Exit)
        {
            _global.DataCompleted();
        }
    }

    public double Throughput =>
        _global.Throughput;

    public TimeSpan GlobalAverageLatency =>
        _global.AverageLatency;

    public TimeSpan GlobalMaxLatency =>
        _global.MaxLatency;

    internal IReadOnlyList<PipelineSegmentMetrics> Segments =>
            _segments;
}