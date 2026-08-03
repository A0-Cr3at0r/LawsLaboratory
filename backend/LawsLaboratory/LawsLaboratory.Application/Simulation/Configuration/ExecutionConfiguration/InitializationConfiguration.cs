namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration
{
    public sealed class InitializationConfiguration
    {
        public required RealDistributionConfiguration InitialValueDistribution { get; init; }

        public SpatialDistributionConfiguration? InitialSpatialDistribution { get; init; }

        public DomainConfiguration? DomainConfiguration { get; init; }

        public int? TargetCellCount { get; init; }
    }
}
