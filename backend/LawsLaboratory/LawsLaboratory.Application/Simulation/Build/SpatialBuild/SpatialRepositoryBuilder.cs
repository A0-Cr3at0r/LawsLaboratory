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