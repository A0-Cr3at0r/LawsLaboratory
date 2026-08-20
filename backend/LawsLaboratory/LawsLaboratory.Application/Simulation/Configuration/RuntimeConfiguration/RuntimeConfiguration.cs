// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Configuration / RuntimeConfigurationPack
//
// RuntimeConfiguration.cs
//
// Defines the runtime configuration of a simulation.
//
// Runtime configuration describes the execution environment of the simulation,
// independently from the model being simulated. It groups grid and time
// settings used when executing the configured model.
//
// These settings contain configuration data only and are consumed by the
// simulation initialization and execution infrastructure.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;

public sealed record RuntimeConfiguration
{
    public required TimeConfiguration Time { get; init; }

    public required GridConfiguration Grid { get; init; }
}