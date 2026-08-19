// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / EnvironnementRepository / Spatial
//
// SpatialAccess.cs
//
// Represents one elementary spatial dependency required during execution.
//
// An access identifies:
//
//     ParameterId  → parameter to read or write
//     CellOffset   → relative position of the target cell in the linearized grid
//
// SpatialAccess is intentionally a small value type because spatial execution
// plans may contain many such entries and are intended to be reused during
// simulation execution.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;

internal readonly struct SpatialAccess
{
    public ushort ParameterId { get; }

    public int CellOffset { get; }


    public SpatialAccess(
        ushort parameterId,
        int cellOffset)
    {
        ParameterId = parameterId;
        CellOffset = cellOffset;
    }
}