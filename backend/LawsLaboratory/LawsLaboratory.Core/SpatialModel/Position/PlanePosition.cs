// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / SpatialModel / Position
//
// PlanePosition.cs
//
// Represents a position in a two-dimensional plane.
//
// PlanePosition itself does not define whether the coordinates are absolute
// or relative. Their interpretation is determined by the component using them.
// -----------------------------------------------------------------------------

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