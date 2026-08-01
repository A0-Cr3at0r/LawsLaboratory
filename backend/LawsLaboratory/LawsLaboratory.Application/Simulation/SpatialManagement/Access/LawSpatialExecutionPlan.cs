namespace LawsLaboratory.Application.Simulation.SpatialManagement.Access;

internal readonly struct LawSpatialExecutionPlan
{
    internal SpatialAccessPlan VariationAccessPlan { get; }

    internal SpatialAccessPlan TransmissionSourceAccessPlan { get; }

    internal SpatialAccessPlan TransmissionDestinationAccessPlan { get; }


    internal LawSpatialExecutionPlan(
        SpatialAccessPlan variationAccessPlan,
        SpatialAccessPlan transmissionSourceAccessPlan,
        SpatialAccessPlan transmissionDestinationAccessPlan)
    {
        VariationAccessPlan = variationAccessPlan;
        TransmissionSourceAccessPlan = transmissionSourceAccessPlan;
        TransmissionDestinationAccessPlan = transmissionDestinationAccessPlan;
    }
}