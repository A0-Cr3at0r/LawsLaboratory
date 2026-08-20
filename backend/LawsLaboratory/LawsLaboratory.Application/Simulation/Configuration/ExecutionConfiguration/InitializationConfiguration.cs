// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Configuration / ExecutionConfiguration
//
// InitializationConfiguration.cs
//
// Defines the declarative configuration of simulation initialization.
//
// It specifies the distribution used to generate initial parameter values,
// the optional spatial distribution and domain used to place cells, and the
// target number of cells to initialize.
//
// The Initializer and its builders consume  this configuration to construct the runtime
// initialization rule used by the simulation engine.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;

public sealed record InitializationConfiguration
{
    public required RealDistributionConfiguration InitialValueDistribution { get; init; }

    public SpatialDistributionConfiguration? InitialSpatialDistribution { get; init; }

    public DomainConfiguration? DomainConfiguration { get; init; }

    public required int TargetCellCount { get; init; }
}
