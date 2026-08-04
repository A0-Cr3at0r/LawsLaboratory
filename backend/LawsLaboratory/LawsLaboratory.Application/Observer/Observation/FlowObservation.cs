namespace LawsLaboratory.Application.Observer.Observation;

public enum FlowDirection
{
    Enter,
    Exit
}

internal  record struct FlowObservation
{
    public MonitorChannel Channel { get; set; }

    public FlowDirection Direction { get; set; }

    public ushort? ParameterId { get; set; }

    public FlowObservation(
        MonitorChannel channel,
        FlowDirection direction,
        ushort? parameterId = null)
    {
        Channel = channel;
        Direction = direction;
        ParameterId = parameterId;
    }
}