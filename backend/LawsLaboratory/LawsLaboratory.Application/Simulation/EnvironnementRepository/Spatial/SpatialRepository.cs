namespace LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;


internal sealed class SpatialRepository
{
    private readonly Dictionary<ushort, LawSpatialExecutionPlan> _plans;

    public SpatialRepository(
        Dictionary<ushort, LawSpatialExecutionPlan> plans)
    {
        _plans = plans;
    }

    public LawSpatialExecutionPlan GetPlan(
        ushort targetParameterId)
    {
        return _plans[targetParameterId];
    }
}