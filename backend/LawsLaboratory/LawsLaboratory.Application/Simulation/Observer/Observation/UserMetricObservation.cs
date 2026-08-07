namespace LawsLaboratory.Application.Simulation.Observer.Observation;

public  record struct UserMetricObservation
{
    public ushort ParameterId { get; set; }

    public double? Value { get; set; }


    public UserMetricObservation(
        ushort parameterId,
        double? value = null)
    {
        ParameterId = parameterId;
        Value = value;
    }
}