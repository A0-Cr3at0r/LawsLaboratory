using LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;
using LawsLaboratory.Core.Mathematics.Domain;
using LawsLaboratory.Core.Mathematics.Domain.CompositeDomain;
using LawsLaboratory.Core.Mathematics.Domain.GeometricDomain;
using System.Numerics;

namespace LawsLaboratory.Application.Simulation.Build.InitializationBuild;

internal sealed class DomainFactory
{
    public IDomain<Vector2>? Create(
        DomainConfiguration? configuration)
    {
        if (configuration is null)
            return null;


        return configuration switch
        {
            BoxDomainConfiguration box =>
                CreateBox(box),


            EllipseDomainConfiguration ellipse =>
                new EllipseDomain(
                    ToVector2(ellipse.Focus1),
                    ToVector2(ellipse.Focus2),
                    ellipse.MajorAxis),


            HyperbolaDomainConfiguration hyperbola =>
                new HyperbolaDomain(
                    ToVector2(hyperbola.Focus1),
                    ToVector2(hyperbola.Focus2),
                    hyperbola.DistanceDifference),


            ParabolaDomainConfiguration parabola =>
                new ParabolaDomain(
                    ToVector2(parabola.Focus),
                    parabola.A,
                    parabola.B,
                    parabola.C,
                    parabola.IncludeCloserSide),


            HalfPlaneDomainConfiguration halfPlane =>
                new HalfPlaneDomain(
                    halfPlane.A,
                    halfPlane.B,
                    halfPlane.C),


            PolygonDomainConfiguration polygon =>
                new PolygonDomain(
                    polygon.Vertices
                        .Select(ToVector2)),


            UnionDomainConfiguration union =>
                new UnionDomain<Vector2>(
                    union.Domains
                        .Select(Create)
                        .OfType<IDomain<Vector2>>()),


            IntersectionDomainConfiguration intersection =>
                new IntersectionDomain<Vector2>(
                    intersection.Domains
                        .Select(Create)
                        .OfType<IDomain<Vector2>>()),


            ComplementDomainConfiguration complement =>
                new ComplementDomain<Vector2>(
                    Create(complement.Domain)
                    ??
                    throw new InvalidOperationException()),


            _ =>
                throw new NotSupportedException(
                    $"Unsupported domain configuration: {configuration.GetType().Name}")
        };
    }



    private static BoxDomain CreateBox(
        BoxDomainConfiguration configuration)
    {
        return new BoxDomain(
            configuration.MinimumX,
            configuration.MaximumX,
            configuration.MinimumY,
            configuration.MaximumY);
    }



    private static Vector2 ToVector2(
        Vector2Configuration configuration)
    {
        return new Vector2(
            (float)configuration.X,
            (float)configuration.Y);
    }
}