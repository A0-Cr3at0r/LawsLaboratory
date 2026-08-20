// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Discretizer
//
// Vector2RoundDiscretizer.cs
//
// Discretizes a two-dimensional Vector2 position by independently rounding
// its X and Y coordinates to the nearest integer.
//
// The resulting integer coordinates are represented by PlanePosition,
// providing the bridge between continuous spatial representations and the
// discrete planar representation used by the grid.
// -----------------------------------------------------------------------------

using LawsLaboratory.Core.SpatialModel.Position;
using System.Numerics;

namespace LawsLaboratory.Core.Mathematics.Discretizer;

public sealed class Vector2RoundDiscretizer
    : IDiscretizer<Vector2, PlanePosition>
{
    public PlanePosition Discretize(Vector2 value)
    {
        return new PlanePosition(
            (int)Math.Round(value.X),
            (int)Math.Round(value.Y));
    }
}