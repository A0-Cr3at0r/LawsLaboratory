// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / EnvironnementRepository / Spatial
//
// SpatialRepository.cs
//
// Provides indexed access to the precomputed spatial execution plan of each
// target parameter.
//
// Each ParameterId identifies the Law whose spatial dependencies must be
// consulted during execution.
//
// The repository separates the construction and storage of spatial execution
// plans from the runtime components that consume them.
//
// The supplied dictionary is defensively copied during construction. The
// repository is therefore immutable after construction: modifications to the
// original dictionary cannot alter the stored execution plans.
//
// Spatial execution plans are precomputed before runtime and are treated as
// read-only execution metadata by the simulation pipeline.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;

internal sealed class SpatialRepository
{
    private readonly Dictionary<ushort, LawSpatialExecutionPlan> _plans;

    public SpatialRepository(
        Dictionary<ushort, LawSpatialExecutionPlan> plans)
    {
        ArgumentNullException.ThrowIfNull(plans);

        _plans = new Dictionary<ushort, LawSpatialExecutionPlan>(plans);
    }

    public LawSpatialExecutionPlan GetPlan(
        ushort targetParameterId)
    {
        return _plans[targetParameterId];
    }
}