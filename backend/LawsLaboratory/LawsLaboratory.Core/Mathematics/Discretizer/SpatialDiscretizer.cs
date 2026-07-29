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