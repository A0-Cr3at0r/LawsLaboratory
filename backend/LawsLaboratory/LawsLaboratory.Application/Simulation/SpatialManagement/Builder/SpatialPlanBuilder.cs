using LawsLaboratory.Application.Simulation.SpatialManagement.Access;
using LawsLaboratory.Core.Laws;
using LawsLaboratory.Core.SpatialModel.Position;

namespace LawsLaboratory.Application.Simulation.SpatialManagement.Builder;

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