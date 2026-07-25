using LawsLaboratory.Core.Mathematics.Distributions;
using LawsLaboratory.Core.Mathematics.Domain;

namespace LawsLaboratory.Core.Laws;

public sealed class InitializationRule
{
    public int TargetCellCount { get; }

    public IDistribution<double> Distribution { get; }

    public IValidDomain<double>? ValidDomain { get; }

    public InitializationRule(
        int targetCellCount,
        IDistribution<double> distribution,
        IValidDomain<double>? validDomain = null)
    {
        TargetCellCount = targetCellCount;
        Distribution = distribution;
        ValidDomain = validDomain;
    }
}