using LawsLaboratory.Application.Observer;
using LawsLaboratory.Application.Simulation.SpatialManagement.Access;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Core.Value;

internal sealed class SpatialReader
{
    private readonly IGrid<PlanePosition> _grid;
    private readonly IValue[] _values;
    private readonly ObservationDispatcher _observer;

    public SpatialReader(
        IGrid<PlanePosition> grid,
        ObservationDispatcher observer,
        int maxVariableCount)
    {
        _grid = grid;
        _observer = observer;

        _values = new IValue[maxVariableCount];

        for (int i = 0; i < maxVariableCount; i++)
        {
            _values[i] = Dead.Instance;
        }
    }

    public ReadOnlySpan<IValue> Values => _values;

    public bool TryRead(
        int cellId,
        SpatialAccessPlan accessPlan,
        ushort currentParameterId)
    {
        IValue currentValue =
            _grid.GetParameterValue(cellId, currentParameterId);

        _observer.EmitMetric(
            currentParameterId,
            currentValue.Get());

        if (ReferenceEquals(currentValue, Dead.Instance))
        {
            return false;
        }

        for (int i = 0; i < accessPlan.Count; i++)
        {
            SpatialAccess access = accessPlan.GetAccess(i);

            _values[i] = _grid.GetParameterValue(
                cellId + access.CellOffset,
                access.ParameterId);
        }

        return true;
    }
}