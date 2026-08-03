using LawsLaboratory.Core.Mathematics.Distributions;
using LawsLaboratory.Core.Mathematics.Domain;
using System.Numerics;

namespace LawsLaboratory.Core.Laws;

public sealed class InitializationRule
{
    public int? TargetCellCount { get; }

    public IDistribution<double> ValueDistribution { get; }

    public IDistribution<Vector2>? SpaceDistribution { get; }
    
    public IDomain<Vector2>? SpaceDomain { get; }

    public bool HasSpatialSelection =>
        SpaceDistribution != null;

    public InitializationRule(
        IDistribution<double> distribution,
        int? targetCellCount = null,
        IDistribution<Vector2>? spaceDistribution = null,
        IDomain<Vector2>? validDomain = null)
    {
        TargetCellCount = targetCellCount;
        ValueDistribution = distribution;
        SpaceDistribution = spaceDistribution;
        SpaceDomain = validDomain;
    }
}