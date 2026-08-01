namespace LawsLaboratory.Core.SpatialModel.Grid;

using LawsLaboratory.Core.SpatialModel.Boundary;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Core.Value;

public sealed class PlaneGrid : IGrid<PlanePosition>
{
    private readonly Cell[] _cells;

    private readonly int _width;
    private readonly int _height;

    private readonly IBoundaryCondition<PlanePosition> _boundary;


    public int Dimension => 2;

    public int Size => _cells.Length;


    public PlaneGrid(
        int width,
        int height,
        int parameterCount,
        IBoundaryCondition<PlanePosition> boundary)
    {
        _width = width;
        _height = height;

        _boundary = boundary;

        _cells = new Cell[width * height];


        for (int i = 0; i < _cells.Length; i++)
        {
            _cells[i] = new Cell(i, parameterCount);
        }
    }

    public IValue GetParameterValue(int cellId,  ushort parameterId)
    {
        return _cells[cellId].GetParameterValue(parameterId);
    }

    public void SetCellParameterValue(int cellId, ushort parameterId, IValue value)
    {
        _cells[cellId].SetParameterValue(parameterId, value);
    }

    public void SetCellParameterValue(int cellId, ushort parameterId, double value)
    {
        _cells[cellId].SetParameterValue(parameterId, value);
    }


    public PlanePosition GetPosition(int id)
    {
        int x = id % _width;
        int y = id / _width;

        return new PlanePosition(x, y);
    }


    public bool Contains(PlanePosition position)
    {
        return
            position.X >= 0 &&
            position.X < _width &&
            position.Y >= 0 &&
            position.Y < _height;
    }


    public IValue GetParameterValue(PlanePosition position, ushort parameterId)
    {
        PlanePosition resolved = _boundary.Resolve(position);

        int cellId = ToId(resolved);

        return GetParameterValue(cellId, parameterId);
    }


    private int ToId(PlanePosition position)
    {
        return position.Y * _width + position.X;
    }
}