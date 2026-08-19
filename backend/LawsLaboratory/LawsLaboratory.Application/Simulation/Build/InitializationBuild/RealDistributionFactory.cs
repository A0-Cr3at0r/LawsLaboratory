using LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;
using LawsLaboratory.Core.Mathematics.Distributions;
using LawsLaboratory.Core.Mathematics.Distributions.DiscreteDistributions;
using LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Application.Simulation.Build.InitializationBuild;

internal sealed class RealDistributionFactory
{
    private readonly IRandomGenerator _random;


    public RealDistributionFactory(
        IRandomGenerator random)
    {
        ArgumentNullException.ThrowIfNull(random);

        _random = random;
    }


    public IDistribution<double> Create(
        RealDistributionConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return configuration switch
        {
            ContinuousDistributionConfiguration continuous
                => CreateContinuousDistribution(continuous),

            DiscreteDistributionConfiguration discrete
                => CreateDiscreteDistribution(discrete),

            _ => throw new NotSupportedException(
                $"Unsupported configuration type '{configuration.GetType().Name}'.")
        };
    }


    private IDistribution<double> CreateContinuousDistribution(
        ContinuousDistributionConfiguration configuration)
    {
        return configuration switch
        {
            ConstantDistributionConfiguration c =>
                new ConstantDistribution(c.Value),

            UniformDistributionConfiguration u =>
                new UniformDistribution(
                    u.Minimum,
                    u.Maximum,
                    _random),

            NormalDistributionConfiguration n =>
                new NormalDistribution(
                    n.Mean,
                    n.StandardDeviation,
                    _random),

            LogNormalDistributionConfiguration l =>
                new LogNormalDistribution(
                    l.Mean,
                    l.StandardDeviation,
                    _random),

            StudentTDistributionConfiguration s =>
                new StudentTDistribution(
                    s.DegreesOfFreedom,
                    _random),

            GammaDistributionConfiguration g =>
                new GammaDistribution(
                    g.Shape,
                    g.Scale,
                    _random),

            BetaDistributionConfiguration b =>
                new BetaDistribution(
                    b.Alpha,
                    b.Beta,
                    _random),

            ExponentialDistributionConfiguration e =>
                new ExponentialDistribution(
                    e.Lambda,
                    _random),

            LaplaceDistributionConfiguration l =>
                new LaplaceDistribution(
                    l.Mean,
                    l.Scale,
                    _random),

            TriangularDistributionConfiguration t =>
                new TriangularDistribution(
                    t.Minimum,
                    t.Mode,
                    t.Maximum,
                    _random),

            CauchyDistributionConfiguration c =>
                new CauchyDistribution(
                    c.Location,
                    c.Scale,
                    _random),

            WeibullDistributionConfiguration w =>
                new WeibullDistribution(
                    w.Shape,
                    w.Scale,
                    _random),

            GumbelDistributionConfiguration g =>
                new GumbelDistribution(
                    g.Location,
                    g.Scale,
                    _random),

            RayleighDistributionConfiguration r =>
                new RayleighDistribution(
                    r.Scale,
                    _random),

            ParetoDistributionConfiguration p =>
                new ParetoDistribution(
                    p.Minimum,
                    p.Shape,
                    _random),

            GeneralizedBetaDistributionConfiguration b =>
                new GeneralizedBetaDistribution(
                    b.A,
                    b.B - b.A,
                    b.Alpha,
                    b.Beta,
                    _random),

            _ => throw new NotSupportedException(
                $"Unsupported configuration type '{configuration.GetType().Name}'.")
        };
    }


    private IDistribution<double> CreateDiscreteDistribution(
        DiscreteDistributionConfiguration configuration)
    {
        return configuration switch
        {
            BernoulliDistributionConfiguration b =>
                new DoubleDistributionAdapter<int>(
                    new BernoulliDistribution(
                        b.Probability,
                        _random)),

            BinomialDistributionConfiguration b =>
                new DoubleDistributionAdapter<int>(
                    new BinomialDistribution(
                        b.Trials,
                        b.Probability,
                        _random)),

            NegativeBinomialDistributionConfiguration n =>
                new DoubleDistributionAdapter<int>(
                    new NegativeBinomialDistribution(
                        n.SuccessCount,
                        n.Probability,
                        _random)),

            HypergeometricDistributionConfiguration h =>
                new DoubleDistributionAdapter<int>(
                    new HypergeometricDistribution(
                        h.PopulationSize,
                        h.SuccessPopulation,
                        h.SampleSize,
                        _random)),

            PoissonDistributionConfiguration p =>
                new DoubleDistributionAdapter<int>(
                    new PoissonDistribution(
                        p.Lambda,
                        _random)),

            GeometricDistributionConfiguration g =>
                new DoubleDistributionAdapter<int>(
                    new GeometricDistribution(
                        g.Probability,
                        _random)),

            ZipfDistributionConfiguration z =>
                new DoubleDistributionAdapter<int>(
                    new ZipfDistribution(
                        z.Size,
                        z.Exponent,
                        _random)),

            _ => throw new NotSupportedException(
                $"Unsupported configuration type '{configuration.GetType().Name}'.")
        };
    }
}