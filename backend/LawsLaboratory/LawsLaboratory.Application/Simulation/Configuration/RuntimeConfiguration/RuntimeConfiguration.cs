namespace LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;

public sealed class RuntimeConfiguration
{
    public required TimeConfiguration Time { get; init; }

    public required GridConfiguration Grid { get; init; }
}