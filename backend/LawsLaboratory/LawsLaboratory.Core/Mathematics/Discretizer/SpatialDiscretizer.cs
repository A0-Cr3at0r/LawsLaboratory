using LawsLaboratory.Core.SpatialModel;
using System.Numerics;

namespace LawsLaboratory.Core.Mathematics.Discretizer;

public sealed class Vector2RoundDiscretizer
    : IDiscretizer<Vector2, Position>
{
    public Position Discretize(Vector2 value)
    {
        return new Position(
            (int)Math.Round(value.X),
            (int)Math.Round(value.Y));
    }
}