// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Laws
//
// InitializationRule.cs
//
// Defines how the initial state of a parameter is generated.
//
// ValueDistribution determines the values assigned to the selected cells.
// TargetCellCount optionally limits the number of cells to initialize.
//
// SpaceDistribution optionally determines the spatial positions at which
// initialization is attempted. Its coordinates are interpreted relative to
// the origin of the simulation plane.
//
// SpaceDomain optionally constrains the spatial region in which initialization
// is allowed to occur. It acts as a geometric constraint on positions produced
// by the spatial distribution rather than defining a spatial distribution
// itself.
// -----------------------------------------------------------------------------

using LawsLaboratory.Core.Mathematics.Distributions;
using LawsLaboratory.Core.Mathematics.Domain;
using System.Numerics;

namespace LawsLaboratory.Core.Laws;

public sealed class InitializationRule
{
    public int TargetCellCount { get; }

    public IDistribution<double> ValueDistribution { get; }

    public IDistribution<Vector2>? SpaceDistribution { get; }
    
    public IDomain<Vector2>? SpaceDomain { get; }

    public bool HasSpatialSelection =>
        SpaceDistribution != null;

    public InitializationRule(
        IDistribution<double> distribution,
        int targetCellCount,
        IDistribution<Vector2>? spaceDistribution = null,
        IDomain<Vector2>? validDomain = null)
    {
        TargetCellCount = targetCellCount;
        ValueDistribution = distribution;
        SpaceDistribution = spaceDistribution;
        SpaceDomain = validDomain;
    }
}