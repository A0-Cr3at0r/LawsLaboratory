// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / EnvironnementRepository / Spatial
//
// SpatialAccessPlan.cs
//
// Describes the ordered spatial dependencies required by one execution step.
//
// Each entry identifies a parameter and the relative cell offset from the
// current cell. The plan is consumed by spatial readers and writers to perform
// the required grid accesses without recomputing spatial relationships.
//
// The order of accesses is significant: it defines the correspondence between
// the plan and the value buffer used during expression evaluation.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;

internal sealed class SpatialAccessPlan
{
    private readonly SpatialAccess[] _accesses;


    public int Count => _accesses.Length;


    public SpatialAccessPlan(
        SpatialAccess[] accesses)
    {
        _accesses = accesses;
    }


    public SpatialAccess GetAccess(int index)
    {
        return _accesses[index];
    }
}