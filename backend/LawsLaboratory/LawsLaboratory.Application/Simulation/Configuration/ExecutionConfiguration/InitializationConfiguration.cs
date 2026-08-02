namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration
{
    public sealed class InitializationConfiguration
    {
        public RealDistributionConfiguration InitialValueDistribution { get; init; }

        public SpatialDistributionConfiguration? InitialPositionDistribution { get; init; }

        public DomainConfiguration? DomainConfiguration { get; init; }

        public int TargetCellCount { get; init; }
    }
}
