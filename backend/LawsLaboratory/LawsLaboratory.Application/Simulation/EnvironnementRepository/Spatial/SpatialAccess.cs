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