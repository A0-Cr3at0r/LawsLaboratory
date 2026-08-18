namespace LawsLaboratory.Core.SpatialModel.Boundary;

using LawsLaboratory.Core.SpatialModel.Position;

internal sealed class ClampBoundaryCondition : IBoundaryCondition<PlanePosition>
{
    private readonly int _width;
    private readonly int _height;


    public ClampBoundaryCondition(
        int width,
        int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width));

        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height));

        _width = width;
        _height = height;
    }


    public PlanePosition Resolve(PlanePosition position)
    {
        int x = Clamp(
            position.X,
            0,
            _width - 1);

        int y = Clamp(
            position.Y,
            0,
            _height - 1);

        return new PlanePosition(x, y);
    }

    public int Resolve(int cellID)
    {
        if (cellID < 0)
        {
            return 0;
        }
        else if (cellID >= _width * _height)
        {
            return _width * _height - 1;
        }

        return cellID;
    }



    private static int Clamp(
        int value,
        int min,
        int max)
    {
        if (value < min)
            return min;

        if (value > max)
            return max;

        return value;
    }
}