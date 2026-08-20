// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Configuration / ExecutionConfiguration
//
// ValueDistributionConfiguration.cs
//
// Defines the declarative probability distribution configurations used by
// simulation initialization.
//
// The hierarchy represents continuous and discrete real-valued distributions
// and contains only the parameters required to construct their corresponding
// Core distribution objects.
//
// The Initializer and its builders consume these configurations and delegates parameter
// validation to the constructed Core objects.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;

public abstract record DistributionConfiguration
{
}


public abstract record RealDistributionConfiguration
    : DistributionConfiguration
{
}


public abstract record ContinuousDistributionConfiguration
    : RealDistributionConfiguration
{
}


public abstract record DiscreteDistributionConfiguration
    : RealDistributionConfiguration
{
}


// ============================================================
// Continuous distributions
// ============================================================

public sealed record ConstantDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Value { get; init; }
}


public sealed record UniformDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Minimum { get; init; }

    public double Maximum { get; init; }
}


public sealed record NormalDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Mean { get; init; }

    public double StandardDeviation { get; init; }
}


public sealed record LogNormalDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Mean { get; init; }

    public double StandardDeviation { get; init; }
}


public sealed record StudentTDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double DegreesOfFreedom { get; init; }
}


public sealed record GammaDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Shape { get; init; }

    public double Scale { get; init; }
}


public sealed record BetaDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Alpha { get; init; }

    public double Beta { get; init; }
}


public sealed record ExponentialDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Lambda { get; init; }
}


public sealed record LaplaceDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Mean { get; init; }

    public double Scale { get; init; }
}


public sealed record TriangularDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Minimum { get; init; }

    public double Mode { get; init; }

    public double Maximum { get; init; }
}


public sealed record CauchyDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Location { get; init; }

    public double Scale { get; init; }
}


public sealed record WeibullDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Shape { get; init; }

    public double Scale { get; init; }
}


public sealed record GumbelDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Location { get; init; }

    public double Scale { get; init; }
}


public sealed record RayleighDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Scale { get; init; }
}


public sealed record ParetoDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Minimum { get; init; }

    public double Shape { get; init; }
}


public sealed record GeneralizedBetaDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Alpha { get; init; }

    public double Beta { get; init; }

    public double A { get; init; }

    public double B { get; init; }
}


// ============================================================
// Discrete distributions
// ============================================================

public sealed record BernoulliDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public double Probability { get; init; }
}


public sealed record BinomialDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public int Trials { get; init; }

    public double Probability { get; init; }
}


public sealed record NegativeBinomialDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public int SuccessCount { get; init; }

    public double Probability { get; init; }
}


public sealed record HypergeometricDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public int PopulationSize { get; init; }

    public int SuccessPopulation { get; init; }

    public int SampleSize { get; init; }
}


public sealed record PoissonDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public double Lambda { get; init; }
}


public sealed record GeometricDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public double Probability { get; init; }
}


public sealed record ZipfDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public int Size { get; init; }

    public double Exponent { get; init; }
}


public sealed record MultinomialDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public int Trials { get; init; }

    public required double[] Probabilities { get; init; }
}