// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Configuration / RuntimeConfigurationPack
//
// TimeConfiguration.cs
//
// Defines the temporal execution constraints of a simulation.
//
// The configuration optionally specifies a maximum number of simulation
// cycles and an artificial delay between cycles. These settings affect
// execution behavior but do not belong to the simulation model itself.
//
// This type contains configuration data only and is consumed by the runtime
// execution infrastructure.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;
    public sealed record TimeConfiguration
    {
        public int? MaxCycles { get; init; }

        public int? DelayMsPerCycle { get; init; }
    }

