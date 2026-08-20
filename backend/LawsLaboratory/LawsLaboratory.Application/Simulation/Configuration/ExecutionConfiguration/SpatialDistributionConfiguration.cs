// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Configuration / ExecutionConfiguration
//
// SpatialDistributionConfiguration.cs
//
// Defines the declarative spatial probability distributions used to determine
// where initialized cells are placed within their valid domain.
//
// Configurations describe independent-axis, radial and mixture-based spatial
// distributions. They contain configuration data only and do not generate
// positions themselves.
//
// The Initializer and its builders consume  these configurations to construct the corresponding
// Core spatial distribution objects.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;

public abstract record SpatialDistributionConfiguration
{
}

public sealed record IndependentAxisDistributionConfiguration
: SpatialDistributionConfiguration
{
    public required RealDistributionConfiguration X { get; init; }

    public required RealDistributionConfiguration Y { get; init; }
}

public sealed record RadialDistributionConfiguration
: SpatialDistributionConfiguration
{
    public required RealDistributionConfiguration Radius { get; init; }

    public required RealDistributionConfiguration Angle { get; init; }

}

public sealed record MixtureComponentConfiguration
{
    public double Weight { get; init; }

    public required SpatialDistributionConfiguration Distribution { get; init; }
}

public sealed record MixtureDistributionConfiguration
: SpatialDistributionConfiguration
{
    public required MixtureComponentConfiguration[] Components { get; init; }
}
