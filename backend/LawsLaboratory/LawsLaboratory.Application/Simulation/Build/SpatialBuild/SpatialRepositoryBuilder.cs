// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Build / SpatialBuild
//
// SpatialRepositoryBuilder.cs
//
// Builds the SpatialRepository containing the precomputed spatial execution
// plans required by the simulation engine.
//
// A spatial plan is created for each runtime Law and indexed by its target
// parameter identifier. The resulting repository allows spatial dependencies
// to be resolved without rebuilding execution plans during simulation.
//
// This class prepares runtime spatial data and contains no simulation logic.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;
using LawsLaboratory.Core.Laws;

namespace LawsLaboratory.Application.Simulation.Build.SpatialBuild;

internal sealed class SpatialRepositoryBuilder
{
    private readonly SpatialPlanBuilder _planBuilder;

    public SpatialRepositoryBuilder(
        SpatialPlanBuilder planBuilder)
    {
        _planBuilder = planBuilder;
    }

    public SpatialRepository Build(
        IReadOnlyList<Law> laws,
        int gridWidth)
    {
        Dictionary<ushort, LawSpatialExecutionPlan> plans =
            new(laws.Count);

        foreach (Law law in laws)
        {
            plans.Add(
                law.TargetParameterId,
                _planBuilder.Build(
                    law,
                    gridWidth));
        }

        return new SpatialRepository(plans);
    }
}