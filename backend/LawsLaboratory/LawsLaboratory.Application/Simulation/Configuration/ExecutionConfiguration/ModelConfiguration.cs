namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration
{
    public sealed class ModelConfiguration
    {
        public required Dictionary<string, LawConfiguration> ParametersLaws { get; init; }
    }
}
