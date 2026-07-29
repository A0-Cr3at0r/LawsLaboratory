namespace LawsLaboratory.Core.SpatialModel.Position;

public readonly struct PlanePosition
{
    public int X { get; }

    public int Y { get; }


    public PlanePosition(int x, int y)
    {
        X = x;
        Y = y;
    }


    public static PlanePosition operator +
        (PlanePosition left, PlanePosition right)
    {
        return new PlanePosition(
            left.X + right.X,
            left.Y + right.Y
        );
    }
}