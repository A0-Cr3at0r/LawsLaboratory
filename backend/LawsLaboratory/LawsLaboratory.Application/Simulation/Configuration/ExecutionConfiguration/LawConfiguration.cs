namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;

public sealed class LawConfiguration
{
    public string VariationFormula { get; init; }

    public string TransmissionFormula { get; init; }

    public InitializationConfiguration InitializationConfiguration { get; init; }

    public PlanePositionConfiguration[] TransmissionDestinations { get; init; }
}

public sealed class PlanePositionConfiguration
{
    public int X { get; init; }
    public int Y { get; init; }
}
