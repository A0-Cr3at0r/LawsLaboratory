using LawsLaboratory.Core.Mathematics.Distributions;

namespace LawsLaboratory.Application.Simulation.Build.InitializationBuild;

internal sealed class DoubleDistributionAdapter<T>
    : IDistribution<double>
{
    private readonly IDistribution<T> _distribution;

    public DoubleDistributionAdapter(
        IDistribution<T> distribution)
    {
        ArgumentNullException.ThrowIfNull(distribution);

        _distribution = distribution;
    }

    public double Generate()
    {
        return Convert.ToDouble(
            _distribution.Generate());
    }
}