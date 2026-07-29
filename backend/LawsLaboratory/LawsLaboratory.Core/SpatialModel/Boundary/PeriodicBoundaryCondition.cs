namespace LawsLaboratory.Core.SpatialModel.Boundary;

using LawsLaboratory.Core.SpatialModel.Position;

internal sealed class PeriodicBoundaryCondition : IBoundaryCondition<PlanePosition>
{
    private readonly int _width;
    private readonly int _height;


    public PeriodicBoundaryCondition(int width, int height)
    {
        _width = width;
        _height = height;
    }


    public PlanePosition Resolve(PlanePosition position)
    {
        int x = Mod(position.X, _width);
        int y = Mod(position.Y, _height);

        return new PlanePosition(x, y);
    }


    private static int Mod(int value, int modulo)
    {
        int result = value % modulo;
        return result < 0 ? result + modulo : result;
    }
}