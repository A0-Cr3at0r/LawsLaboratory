using LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;
using LawsLaboratory.Core.Laws;
using LawsLaboratory.Core.Mathematics.Distributions;
using LawsLaboratory.Core.Mathematics.Domain;
using LawsLaboratory.Core.Mathematics.RandomGenerators;
using System.Numerics;

namespace LawsLaboratory.Application.Simulation.Build.InitializationBuild;

internal sealed class InitializationBuilder
{   
    private readonly SystemRandomGenerator _random = new SystemRandomGenerator();
    private readonly RealDistributionFactory _realDistributionFactory;
    private readonly SpatialDistributionFactory _spatialDistributionFactory;
    private readonly DomainFactory _domainFactory;


    public InitializationBuilder()
    {
        _realDistributionFactory = new RealDistributionFactory(_random);
        _spatialDistributionFactory = new SpatialDistributionFactory(_random);
        _domainFactory = new DomainFactory();
    }


    public InitializationRule Build(
        InitializationConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);


        IDistribution<double> valueDistribution =
            _realDistributionFactory.Create(
                configuration.InitialValueDistribution);


        IDistribution<Vector2>? spatialDistribution =
            configuration.InitialSpatialDistribution != null
            ?
            _spatialDistributionFactory.Create(
                configuration.InitialSpatialDistribution)
            :
            null;


        IDomain<Vector2>? domain =
            configuration.DomainConfiguration != null
            ?
            _domainFactory.Create(
                configuration.DomainConfiguration)
            :
            null;


        return new InitializationRule(
            valueDistribution,
            configuration.TargetCellCount,
            spatialDistribution,
            domain);
    }
}