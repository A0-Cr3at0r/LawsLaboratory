namespace LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;

public sealed class RuntimeConfiguration
{
    public TimeConfiguration Time { get; init; }

    public GridConfiguration Grid { get; init; }
}