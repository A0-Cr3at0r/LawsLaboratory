// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Configuration / ExecutionConfiguration
//
// DomainConfiguration.cs
//
// Defines the declarative spatial domain configurations used to determine
// where simulation cells may be initialized.
//
// The file contains basic geometric domains, composite domains and supporting
// two-dimensional position data. Domains describe valid regions in the
// simulation plane; they do not define the simulation grid itself and contain
// no runtime execution logic.
//
// The Initializer and its builders consume these configurations to construct the corresponding
// Core domain and spatial objects.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;


public abstract record DomainConfiguration
{
}


// ============================================================
// Basic geometric domains
// ============================================================

public sealed record GridDomainConfiguration
    : DomainConfiguration
{
    public int Width { get; init; }

    public int Height { get; init; }
}


public sealed record IntervalDomainConfiguration
    : DomainConfiguration
{
    public double Minimum { get; init; }

    public double Maximum { get; init; }
}


public sealed record BoxDomainConfiguration
    : DomainConfiguration
{
    public double MinimumX { get; init; }

    public double MaximumX { get; init; }

    public double MinimumY { get; init; }

    public double MaximumY { get; init; }
}


public sealed record EllipseDomainConfiguration
    : DomainConfiguration
{
    public Vector2Configuration Focus1 { get; init; } = null!;

    public Vector2Configuration Focus2 { get; init; } = null!;

    public double MajorAxis { get; init; }
}


public sealed record HyperbolaDomainConfiguration
    : DomainConfiguration
{
    public Vector2Configuration Focus1 { get; init; } = null!;

    public Vector2Configuration Focus2 { get; init; } = null!;

    public double DistanceDifference { get; init; }
}


public sealed record ParabolaDomainConfiguration
    : DomainConfiguration
{
    public Vector2Configuration Focus { get; init; } = null!;

    public double A { get; init; }

    public double B { get; init; }

    public double C { get; init; }

    public bool IncludeCloserSide { get; init; } = true;
}


public sealed record HalfPlaneDomainConfiguration
    : DomainConfiguration
{
    public double A { get; init; }

    public double B { get; init; }

    public double C { get; init; }
}


public sealed record PolygonDomainConfiguration
    : DomainConfiguration
{
    public Vector2Configuration[] Vertices { get; init; } = [];
}


// ============================================================
// Composite domains
// ============================================================

public sealed record UnionDomainConfiguration
    : DomainConfiguration
{
    public DomainConfiguration[] Domains { get; init; } = [];
}


public sealed record IntersectionDomainConfiguration
    : DomainConfiguration
{
    public DomainConfiguration[] Domains { get; init; } = [];
}


public sealed record ComplementDomainConfiguration
    : DomainConfiguration
{
    public DomainConfiguration Domain { get; init; } = null!;
}


// ============================================================
// Supporting configurations
// ============================================================

public sealed record Vector2Configuration
{
    public double X { get; init; }

    public double Y { get; init; }
}

