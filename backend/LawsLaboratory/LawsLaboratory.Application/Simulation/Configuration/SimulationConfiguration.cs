// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Configuration
//
// SimulationConfiguration.cs
//
// Defines the root configuration of a simulation.
//
// A simulation configuration combines the declarative model describing the
// simulated laws and initialization with the runtime settings controlling
// how the simulation is executed.
//
// This type is the top-level configuration contract exchanged with the
// frontend and persisted for later reuse.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;
using LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;

namespace LawsLaboratory.Application.Simulation.Configuration;

public record SimulationConfiguration
{
    public required ModelConfiguration Model { get; init; }

    public required RuntimeConfiguration Runtime { get; init; }

}