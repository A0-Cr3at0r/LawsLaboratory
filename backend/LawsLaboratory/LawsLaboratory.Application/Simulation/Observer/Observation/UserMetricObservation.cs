// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Observer / Observation
//
// UserMetricObservation.cs
//
// Represents the observation of one parameter value for one simulation cell.
//
// Value is nullable because a cell may be observed while its parameter is Dead.
// Such an observation still contributes to the received-cell count, while only
// valid values are incorporated into numerical statistics.
// -----------------------------------------------------------------------------

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