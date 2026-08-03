namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration
{
    public abstract class SpatialDistributionConfiguration
    {
    }

    public sealed class IndependentAxisDistributionConfiguration
    : SpatialDistributionConfiguration
    {
        public RealDistributionConfiguration X { get; init; }

        public RealDistributionConfiguration Y { get; init; }
    }

    public sealed class RadialDistributionConfiguration
    : SpatialDistributionConfiguration
    {
        public RealDistributionConfiguration Radius { get; init; }

        public RealDistributionConfiguration Angle { get; init; }

    }

    public sealed class MixtureComponentConfiguration
    {
        public double Weight { get; init; }

        public SpatialDistributionConfiguration Distribution { get; init; }
    }

    public sealed class MixtureDistributionConfiguration
    : SpatialDistributionConfiguration
    {
        public MixtureComponentConfiguration[] Components { get; init; }
    }
}
