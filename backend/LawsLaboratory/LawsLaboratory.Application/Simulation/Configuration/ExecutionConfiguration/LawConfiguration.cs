namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;

public sealed class LawConfiguration
{
    public required string VariationFormula { get; init; }

    public required string TransmissionFormula { get; init; }

    public required InitializationConfiguration InitializationConfiguration { get; init; }

    public required PlanePositionConfiguration[] TransmissionDestinations { get; init; }
}

public sealed class PlanePositionConfiguration
{
    public int X { get; init; }
    public int Y { get; init; }
}
