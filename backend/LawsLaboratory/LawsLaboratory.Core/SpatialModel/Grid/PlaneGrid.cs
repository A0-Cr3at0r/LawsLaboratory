// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / SpatialModel / Grid
//
// PlaneGrid.cs
//
// Provides a two-dimensional grid implementation backed by a one-dimensional
// array of cells.
//
// Cell identifiers are resolved cyclically so that identifiers outside the
// valid range wrap around the grid's flattened storage.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.SpatialModel.Grid;

using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Core.Value;

internal sealed class PlaneGrid : IGrid<PlanePosition>
{
    private readonly Cell[] _cells;

    private readonly int _width;
    private readonly int _height;

    public int Dimension => 2;

    public int Size => _cells.Length;


    public PlaneGrid(
        int width,
        int height,
        int parameterCount)
    {
        _width = width;
        _height = height;


        _cells = new Cell[width * height];


        for (int i = 0; i < _cells.Length; i++)
        {
            _cells[i] = new Cell(i, parameterCount);
        }
    }

    public IValue GetParameterValue(int cellId, ushort parameterId)
    {
        return _cells[Resolve(cellId)].GetParameterValue(parameterId);
    }

    public void SetCellParameterValue(int cellId, ushort parameterId, IValue value)
    {
        _cells[Resolve(cellId)].SetParameterValue(parameterId, value);
    }

    public void SetCellParameterValue(int cellId, ushort parameterId, double value)
    {
        _cells[Resolve(cellId)].SetParameterValue(parameterId, value);
    }

    private int Resolve(int cellId)
    {
        if (0 <= cellId && cellId < Size)
        {
            return cellId;
        }

        return cellId < 0 ? cellId % Size + Size : cellId % Size;

    }

}


