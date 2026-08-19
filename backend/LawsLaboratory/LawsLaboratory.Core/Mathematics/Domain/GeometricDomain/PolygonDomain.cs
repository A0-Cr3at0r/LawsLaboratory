// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Domain / GeometricDomain
//
// PolygonDomain.cs
//
// Represents the interior of a polygon and determines whether a point belongs
// to it using the ray casting algorithm.
//
// Reference:
// Shimrat, G. (1962).
// Algorithm 112: Position of Point Relative to Polygon.
// Communications of the ACM, 5(8), 434.
// -----------------------------------------------------------------------------
using System.Numerics;

namespace LawsLaboratory.Core.Mathematics.Domain.GeometricDomain;


public sealed class PolygonDomain : IDomain<Vector2>
{
    private readonly Vector2[] _vertices;

    public PolygonDomain(IEnumerable<Vector2> vertices)
    {
        _vertices = vertices.ToArray();

        if (_vertices.Length < 3)
            throw new ArgumentException(
                "A polygon requires at least three vertices.");
    }


    public bool Contains(Vector2 point)
    {
        bool inside = false;

        int count = _vertices.Length;

        for (int i = 0, j = count - 1;
             i < count;
             j = i++)
        {
            Vector2 current = _vertices[i];
            Vector2 previous = _vertices[j];

            bool crosses =
                (current.Y > point.Y) !=
                (previous.Y > point.Y);

            if (crosses)
            {
                double xIntersection =
                    previous.X +
                    (point.Y - previous.Y) *
                    (current.X - previous.X) /
                    (current.Y - previous.Y);

                if (point.X < xIntersection)
                {
                    inside = !inside;
                }
            }
        }

        return inside;
    }
}