// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Observer
//
// ObservationDispatcher.cs
//
// Distributes user-metric observations to the registered observers.
//
// The dispatcher is a synchronous fan-out mechanism. It does not control
// simulation execution, synchronize simulation phases, buffer observations, or
// perform asynchronous processing.
//
// User-metric observations are emitted while simulation data is read and are
// forwarded to every registered observer in subscription order.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Simulation.Observer.Observation;

namespace LawsLaboratory.Application.Simulation.Observer;

internal sealed class ObservationDispatcher
{
    private readonly List<IDataObserver<UserMetricObservation>> _observers = [];

    private UserMetricObservation _observation;


    public void Subscribe(
        IDataObserver<UserMetricObservation> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        _observers.Add(observer);
    }


    public void EmitMetric(
        ushort parameterId,
        double? value)
    {
        _observation.ParameterId = parameterId;
        _observation.Value = value;

        foreach (var observer in _observers)
        {
            observer.Notify(_observation);
        }
    }
}