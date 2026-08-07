namespace LawsLaboratory.Application.Simulation.SpatialManagement.ReaderWriter;

using LawsLaboratory.Application.Simulation.SpatialManagement.Access;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Core.Value;

internal sealed class SpatialWriter
{
    private readonly IGrid<PlanePosition> _grid;


    public SpatialWriter(
        IGrid<PlanePosition> grid)
    {
        _grid = grid;
    }

    public void Write(
        int cellId,
        ushort parameterId,
        IValue value)
    {
        _grid.SetCellParameterValue(
            cellId,
            parameterId,
            value);
    }


    public void Write(
        int cellId,
        SpatialAccessPlan accessPlan,
        IValue value)
    {
        for (int i = 0; i < accessPlan.Count; i++)
        {
            SpatialAccess access =
                accessPlan.GetAccess(i);

            _grid.SetCellParameterValue(
                cellId + access.CellOffset, 
                access.ParameterId, 
                value); 

        }
    }
}