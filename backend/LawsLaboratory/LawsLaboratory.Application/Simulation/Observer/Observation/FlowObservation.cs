using LawsLaboratory.Application.Simulation.Observer;

namespace LawsLaboratory.Application.Simulation.Observer.Observation;

public enum FlowDirection
{
    Enter,
    Exit
}

public  record struct FlowObservation
{
    public ObserverChannel Channel { get; set; }

    public FlowDirection Direction { get; set; }

    public ushort? ParameterId { get; set; }

    public FlowObservation(
        ObserverChannel channel,
        FlowDirection direction,
        ushort? parameterId = null)
    {
        Channel = channel;
        Direction = direction;
        ParameterId = parameterId;
    }
}