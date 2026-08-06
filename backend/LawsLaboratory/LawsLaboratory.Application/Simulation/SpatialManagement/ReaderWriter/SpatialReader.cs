using LawsLaboratory.Application.Observer;
using LawsLaboratory.Application.Simulation.SpatialManagement.Access;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Core.Value;

namespace LawsLaboratory.Application.Simulation.SpatialManagement.ReaderWriter;
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

        Array.Fill(_values, Dead.Instance);
    }

    public ReadOnlySpan<IValue> Values => _values;

    public int Count { get; private set; }

    public bool TryRead(
        int cellId,
        SpatialAccessPlan accessPlan,
        ushort currentParameterId)
    {
        Count = 0;

        Array.Fill(_values, Dead.Instance);

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

            IValue value = _grid.GetParameterValue(
                cellId + access.CellOffset,
                access.ParameterId);

            if (ReferenceEquals(value, Dead.Instance))
            {
                return false;
            }

            _values[i] = value;
        }

        Count = accessPlan.Count;

        return true;
    }
}