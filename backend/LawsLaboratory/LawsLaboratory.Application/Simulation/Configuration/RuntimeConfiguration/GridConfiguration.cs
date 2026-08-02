namespace LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;


public enum BoundaryType
{
    Clamp,
    Periodic,
}

public sealed class GridConfiguration
{
    public int Width { get; init; }

    public int Height { get; init; }

    public BoundaryType BoundaryType { get; init; } = BoundaryType.Periodic;
}
