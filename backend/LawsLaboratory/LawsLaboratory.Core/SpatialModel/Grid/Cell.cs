// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / SpatialModel / Grid
//
// Cell.cs
//
// Represents a single cell in the simulation grid.
//
// A cell owns the current value of each registered parameter. It does not
// define spatial coordinates or grid topology; those responsibilities belong
// to the grid and spatial model.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.SpatialModel.Grid;

using LawsLaboratory.Core.Value;

public sealed class Cell
{
    public int Id { get; }

    private readonly IValue[] _values;


    public Cell(int id, int parameterCount)
    {
        Id = id;

        _values = new IValue[parameterCount];

        for (int i = 0; i < parameterCount; i++)
        {
            _values[i] = Dead.Instance;
        }
    }


    internal IValue GetParameterValue(ushort parameterId)
    {
        return _values[parameterId];
    }


    internal void SetParameterValue(
        int parameterId,
        IValue value)
    {
        _values[parameterId] = _values[parameterId].Set(value);
    }

    internal void SetParameterValue(
        ushort parameterId,
        double value)
    {
        _values[parameterId] = _values[parameterId].Set(value);
    }

    internal void KillParameter(ushort parameterId)
    {
        _values[parameterId] = Dead.Instance;
    }
}