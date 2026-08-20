// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Build / SpatialBuild
//
// SpatialPlanBuilder.cs
//
// Builds the precomputed spatial access plans used during simulation
// execution.
//
// Variable references and transmission destinations expressed as relative
// two-dimensional positions are converted into linear grid offsets using the
// configured grid width.
//
// The resulting plans allow runtime readers and writers to access spatial
// dependencies directly without recomputing offsets during execution.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;
using LawsLaboratory.Core.Laws;
using LawsLaboratory.Core.SpatialModel.Position;

namespace LawsLaboratory.Application.Simulation.Build.SpatialBuild;

internal sealed class SpatialPlanBuilder
{
    public LawSpatialExecutionPlan Build(
        Law law,
        int gridWidth)
    {
        SpatialAccessPlan variationPlan =
            BuildAccessPlan(
                law.GetVariationVariables(),
                gridWidth);

        SpatialAccessPlan transmissionSourcePlan =
            BuildAccessPlan(
                law.GetTransmissionVariables(),
                gridWidth);

        SpatialAccessPlan transmissionDestinationPlan =
            BuildDestinationPlan(
                law.GetTransmissionDestinations(),
                law.TargetParameterId,
                gridWidth);

        return new LawSpatialExecutionPlan(
            variationPlan,
            transmissionSourcePlan,
            transmissionDestinationPlan);
    }

    private SpatialAccessPlan BuildAccessPlan(
        IReadOnlyList<VariableReference> variables,
        int gridWidth)
    {
        SpatialAccess[] accesses =
            new SpatialAccess[variables.Count];

        for (int i = 0; i < variables.Count; i++)
        {
            VariableReference variable = variables[i];

            int offset =
                variable.RelativePosition.Y * gridWidth +
                variable.RelativePosition.X;

            accesses[i] = new SpatialAccess(
                variable.ParameterId,
                offset);
        }

        return new SpatialAccessPlan(accesses);
    }

    private SpatialAccessPlan BuildDestinationPlan(
        IReadOnlyList<PlanePosition> destinations,
        ushort targetParameterId,
        int gridWidth)
    {
        SpatialAccess[] accesses =
            new SpatialAccess[destinations.Count];

        for (int i = 0; i < destinations.Count; i++)
        {
            PlanePosition destination = destinations[i];

            int offset =
                destination.Y * gridWidth +
                destination.X;

            accesses[i] = new SpatialAccess(
                targetParameterId,
                offset);
        }

        return new SpatialAccessPlan(accesses);
    }
}