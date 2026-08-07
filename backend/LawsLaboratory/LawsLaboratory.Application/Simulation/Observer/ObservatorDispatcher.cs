using LawsLaboratory.Application.Simulation.Observer.Observation;

namespace LawsLaboratory.Application.Simulation.Observer;

public enum ObserverChannel
{
    RequestBuffer,
    ResultBuffer,
    GatewayExit,
    GatewayEntry,
}

internal sealed class ObservationDispatcher
{
    private readonly List<IDataObserver<FlowObservation>> _flowObservers = [];
    private readonly List<IDataObserver<UserMetricObservation>> _metricObservers = [];

    private FlowObservation _flowObservation;
    private UserMetricObservation _userMetricObservation;

    public ObservationDispatcher()
    {
        _flowObservation = new FlowObservation(
            ObserverChannel.RequestBuffer,
            FlowDirection.Enter);

        _userMetricObservation = new UserMetricObservation(
            parameterId: 1000,
            value: null);
    }


    public void Subscribe(IDataObserver<FlowObservation> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        _flowObservers.Add(observer);
    }

    public void Subscribe(IDataObserver<UserMetricObservation> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        _metricObservers.Add(observer);
    }

    public void EmitFlow(
        ObserverChannel channel,
        FlowDirection direction,
        ushort? parameterId = null)
    {
        
        _flowObservation.Channel = channel;
        _flowObservation.Direction = direction;
        _flowObservation.ParameterId = parameterId;

        foreach (var observer in _flowObservers)
        {
            observer.Notify(_flowObservation);
        }
    }

    public void EmitMetric(
        ushort parameterId,
        double? value)
    {
        _userMetricObservation.ParameterId = parameterId;
        _userMetricObservation.Value = value;

        foreach (var observer in _metricObservers)
        {
            observer.Notify(_userMetricObservation);
        }
    }

    
}

