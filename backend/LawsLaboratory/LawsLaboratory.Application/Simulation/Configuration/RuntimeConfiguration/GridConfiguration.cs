// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Configuration / RuntimeConfigurationPack
//
// GridConfiguration.cs
//
// Defines the dimensions of the simulation grid.
//
// The grid configuration describes the discrete execution space on which the
// simulation operates. It is distinct from DomainConfiguration, which defines
// the spatial region in which cells may be initialized.
//
// This type contains configuration data only and does not represent the
// runtime grid itself.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;

public sealed record GridConfiguration
{
    public int Width { get; init; }

    public int Height { get; init; }

}
