namespace LawsLaboratory.Application.Simulation.SpatialManagement.ReaderWriter;
using LawsLaboratory.Application.Simulation.SpatialManagement.Access;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Core.Value;

internal sealed class SpatialReader
{
    private readonly IGrid<PlanePosition> _grid;

    private readonly IValue[] _values;


    public SpatialReader(
        IGrid<PlanePosition> grid,
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

            _values[i] = 
                _grid.GetParameterValue(
                    cellId + access.CellOffset, 
                    access.ParameterId);

        }

        return _values;
    }
}