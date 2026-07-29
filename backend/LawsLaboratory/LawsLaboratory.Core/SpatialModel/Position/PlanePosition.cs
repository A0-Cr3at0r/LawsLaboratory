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

}