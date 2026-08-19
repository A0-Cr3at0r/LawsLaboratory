// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / SpatialManagement / ReaderWriter
//
// SpatialReader.cs
//
// Reads the values required to evaluate a spatial law from the simulation
// grid.
//
// For a given cell and parameter, the reader first verifies that the current
// value is alive, then follows a precomputed SpatialAccessPlan to collect all
// required spatial dependencies.
//
// Values are written into a reusable fixed-capacity buffer and exposed as a
// ReadOnlySpan to avoid allocations during request preparation.
//
// A request is considered unreadable when the current value or any required
// dependency is Dead. Such cells are therefore excluded from engine requests.
//
// The reader also emits the current parameter value to the observation
// dispatcher for metric collection.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;
using LawsLaboratory.Application.Simulation.Observer;
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