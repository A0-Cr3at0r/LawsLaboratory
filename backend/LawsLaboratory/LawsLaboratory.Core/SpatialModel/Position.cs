namespace LawsLaboratory.Core.SpatialModel;

public readonly struct Position
{
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public Position(int x, int y, int z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }
}