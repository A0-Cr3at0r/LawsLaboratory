namespace LawsLaboratory.Core.Simulation.SpatialManagement;

using LawsLaboratory.Core.Simulation.SpatialManagement.Access;
using LawsLaboratory.Core.SpatialModel;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.Value;

internal sealed class SpatialReader
{
    private readonly PlaneGrid _grid;

    private readonly IValue[] _values;


    public SpatialReader(
        PlaneGrid grid,
        int maxVariableCount)
    {
        _grid = grid;

        _values = new IValue[maxVariableCount];

        for (int i = 0; i < maxVariableCount; i++)
        {
            _values[i] = Dead.Instance;
        }
    }


    public IValue[] Read(
        int cellId,
        SpatialAccessPlan accessPlan)
    {
        for (int i = 0; i < accessPlan.Count; i++)
        {
            SpatialAccess access =
                accessPlan.GetAccess(i);

            Cell cell =
                _grid.GetCell(cellId + access.CellOffset);

            _values[i] =
                cell.GetParameterValue(access.ParameterId);
        }

        return _values;
    }
}