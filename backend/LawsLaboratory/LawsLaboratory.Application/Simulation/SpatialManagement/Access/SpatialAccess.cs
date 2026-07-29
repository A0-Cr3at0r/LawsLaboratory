namespace LawsLaboratory.Application.Simulation.SpatialManagement.Access;

internal readonly struct SpatialAccess
{
    public int ParameterId { get; }

    public int CellOffset { get; }


    public SpatialAccess(
        int parameterId,
        int cellOffset)
    {
        ParameterId = parameterId;
        CellOffset = cellOffset;
    }
}