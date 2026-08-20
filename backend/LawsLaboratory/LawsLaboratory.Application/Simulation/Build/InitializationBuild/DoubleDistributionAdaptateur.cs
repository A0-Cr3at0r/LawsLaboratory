// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Build / InitializationBuild
//
// DoubleDistributionAdapter.cs
//
// Adapts a distribution producing values of type T to the
// IDistribution<double> contract required by simulation initialization.
//
// This adapter is primarily used for discrete distributions whose Core
// implementations produce integral values while initialization consumes
// real-valued distributions.
// -----------------------------------------------------------------------------

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