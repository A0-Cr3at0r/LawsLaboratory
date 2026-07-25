namespace LawsLaboratory.Core.SpatialModel;

public readonly struct RelativePosition
{
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public RelativePosition(int x, int y, int z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }
}