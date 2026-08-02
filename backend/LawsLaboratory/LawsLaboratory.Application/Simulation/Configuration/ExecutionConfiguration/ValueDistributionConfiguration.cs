namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;

public abstract class DistributionConfiguration
{
}


public abstract class RealDistributionConfiguration
    : DistributionConfiguration
{
}


public abstract class ContinuousDistributionConfiguration
    : RealDistributionConfiguration
{
}


public abstract class DiscreteDistributionConfiguration
    : RealDistributionConfiguration
{
}


// ============================================================
// Continuous distributions
// ============================================================

public sealed class ConstantDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Value { get; init; }
}


public sealed class UniformDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Minimum { get; init; }

    public double Maximum { get; init; }
}


public sealed class NormalDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Mean { get; init; }

    public double StandardDeviation { get; init; }
}


public sealed class LogNormalDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Mean { get; init; }

    public double StandardDeviation { get; init; }
}


public sealed class StudentTDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double DegreesOfFreedom { get; init; }
}


public sealed class GammaDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Shape { get; init; }

    public double Scale { get; init; }
}


public sealed class BetaDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Alpha { get; init; }

    public double Beta { get; init; }
}


public sealed class ExponentialDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Lambda { get; init; }
}


public sealed class LaplaceDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Mean { get; init; }

    public double Scale { get; init; }
}


public sealed class TriangularDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Minimum { get; init; }

    public double Mode { get; init; }

    public double Maximum { get; init; }
}


public sealed class CauchyDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Location { get; init; }

    public double Scale { get; init; }
}


public sealed class WeibullDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Shape { get; init; }

    public double Scale { get; init; }
}


public sealed class GumbelDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Location { get; init; }

    public double Scale { get; init; }
}


public sealed class RayleighDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Scale { get; init; }
}


public sealed class ParetoDistributionConfiguration
    : ContinuousDistributionConfiguration
{
    public double Minimum { get; init; }

    public double Shape { get; init; }
}


public sealed class GeneralizedBetaDistributionConfiguration
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

public sealed class BernoulliDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public double Probability { get; init; }
}


public sealed class BinomialDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public int Trials { get; init; }

    public double Probability { get; init; }
}


public sealed class NegativeBinomialDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public int SuccessCount { get; init; }

    public double Probability { get; init; }
}


public sealed class HypergeometricDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public int PopulationSize { get; init; }

    public int SuccessPopulation { get; init; }

    public int SampleSize { get; init; }
}


public sealed class PoissonDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public double Lambda { get; init; }
}


public sealed class GeometricDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public double Probability { get; init; }
}


public sealed class ZipfDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public int Size { get; init; }

    public double Exponent { get; init; }
}


public sealed class MultinomialDistributionConfiguration
    : DiscreteDistributionConfiguration
{
    public int Trials { get; init; }

    public required double[] Probabilities { get; init; }
}