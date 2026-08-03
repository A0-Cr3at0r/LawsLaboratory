using LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;
using LawsLaboratory.Core.Mathematics.Distributions;
using LawsLaboratory.Core.Mathematics.Distributions.SpatialDistribution;
using LawsLaboratory.Core.Mathematics.RandomGenerators;
using System.Numerics;

namespace LawsLaboratory.Application.Simulation.Build.InitializationBuild;

internal sealed class SpatialDistributionFactory
{
    private readonly IRandomGenerator _random;

    private readonly RealDistributionFactory _realDistributionFactory;


    public SpatialDistributionFactory(
        IRandomGenerator random)
    {
        ArgumentNullException.ThrowIfNull(random);

        _random = random;

        _realDistributionFactory =
            new RealDistributionFactory(random);
    }


    public IDistribution<Vector2> Create(
        SpatialDistributionConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return configuration switch
        {
            IndependentAxisDistributionConfiguration independent
                => CreateIndependentAxisDistribution(independent),

            RadialDistributionConfiguration radial
                => CreateRadialDistribution(radial),

            MixtureDistributionConfiguration mixture
                => CreateMixtureDistribution(mixture),

            _ => throw new NotSupportedException(
                $"Unsupported configuration type '{configuration.GetType().Name}'.")
        };
    }


    private IDistribution<Vector2> CreateIndependentAxisDistribution(
        IndependentAxisDistributionConfiguration configuration)
    {
        return new IndependentAxisDistribution(
            _realDistributionFactory.Create(configuration.X),
            _realDistributionFactory.Create(configuration.Y));
    }


    private IDistribution<Vector2> CreateRadialDistribution(
        RadialDistributionConfiguration configuration)
    {
        return new RadialDistribution(
            _realDistributionFactory.Create(configuration.Radius),
            _realDistributionFactory.Create(configuration.Angle));
    }


    private IDistribution<Vector2> CreateMixtureDistribution(
        MixtureDistributionConfiguration configuration)
    {
        IReadOnlyList<IDistribution<Vector2>> distributions =
            configuration.Components
                .Select(component => Create(component.Distribution))
                .ToArray();

        IReadOnlyList<double> weights =
            configuration.Components
                .Select(component => component.Weight)
                .ToArray();

        return new MixtureDistribution<Vector2>(
            distributions,
            weights,
            _random);
    }
}