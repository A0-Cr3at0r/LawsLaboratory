namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration
{
    public abstract class SpatialDistributionConfiguration
    {
    }

    public sealed class IndependentAxisDistributionConfiguration
    : SpatialDistributionConfiguration
    {
        public required RealDistributionConfiguration X { get; init; }

        public required RealDistributionConfiguration Y { get; init; }
    }

    public sealed class RadialDistributionConfiguration
    : SpatialDistributionConfiguration
    {
        public required RealDistributionConfiguration Radius { get; init; }

        public required RealDistributionConfiguration Angle { get; init; }

    }

    public sealed class MixtureComponentConfiguration
    {
        public double Weight { get; init; }

        public required SpatialDistributionConfiguration Distribution { get; init; }
    }

    public sealed class MixtureDistributionConfiguration
    : SpatialDistributionConfiguration
    {
        public required MixtureComponentConfiguration[] Components { get; init; }
    }
}
