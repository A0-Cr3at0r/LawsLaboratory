namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;


public abstract class DomainConfiguration
{
}


// ============================================================
// Basic geometric domains
// ============================================================

public sealed class GridDomainConfiguration
    : DomainConfiguration
{
    public int Width { get; init; }

    public int Height { get; init; }
}


public sealed class IntervalDomainConfiguration
    : DomainConfiguration
{
    public double Minimum { get; init; }

    public double Maximum { get; init; }
}


public sealed class BoxDomainConfiguration
    : DomainConfiguration
{
    public double MinimumX { get; init; }

    public double MaximumX { get; init; }

    public double MinimumY { get; init; }

    public double MaximumY { get; init; }
}


public sealed class EllipseDomainConfiguration
    : DomainConfiguration
{
    public Vector2Configuration Focus1 { get; init; } = null!;

    public Vector2Configuration Focus2 { get; init; } = null!;

    public double MajorAxis { get; init; }
}


public sealed class HyperbolaDomainConfiguration
    : DomainConfiguration
{
    public Vector2Configuration Focus1 { get; init; } = null!;

    public Vector2Configuration Focus2 { get; init; } = null!;

    public double DistanceDifference { get; init; }
}


public sealed class ParabolaDomainConfiguration
    : DomainConfiguration
{
    public Vector2Configuration Focus { get; init; } = null!;

    public double A { get; init; }

    public double B { get; init; }

    public double C { get; init; }

    public bool IncludeCloserSide { get; init; } = true;
}


public sealed class HalfPlaneDomainConfiguration
    : DomainConfiguration
{
    public double A { get; init; }

    public double B { get; init; }

    public double C { get; init; }
}


public sealed class PolygonDomainConfiguration
    : DomainConfiguration
{
    public Vector2Configuration[] Vertices { get; init; } = [];
}


// ============================================================
// Composite domains
// ============================================================

public sealed class UnionDomainConfiguration
    : DomainConfiguration
{
    public DomainConfiguration[] Domains { get; init; } = [];
}


public sealed class IntersectionDomainConfiguration
    : DomainConfiguration
{
    public DomainConfiguration[] Domains { get; init; } = [];
}


public sealed class ComplementDomainConfiguration
    : DomainConfiguration
{
    public DomainConfiguration Domain { get; init; } = null!;
}


// ============================================================
// Supporting configurations
// ============================================================

public sealed class Vector2Configuration
{
    public double X { get; init; }

    public double Y { get; init; }
}

