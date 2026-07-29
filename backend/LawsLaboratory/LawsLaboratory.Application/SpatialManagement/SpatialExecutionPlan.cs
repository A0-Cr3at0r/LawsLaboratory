using LawsLaboratory.Application.SpatialManagement.Access;

namespace LawsLaboratory.Application.SpatialManagement;

internal readonly struct SpatialAccessBundle
{
    internal SpatialAccessPlan VariationAccessPlan { get; }

    internal SpatialAccessPlan TransmissionSourceAccessPlan { get; }

    internal SpatialAccessPlan TransmissionDestinationAccessPlan { get; }


    internal SpatialAccessBundle(
        SpatialAccessPlan variationAccessPlan,
        SpatialAccessPlan transmissionSourceAccessPlan,
        SpatialAccessPlan transmissionDestinationAccessPlan)
    {
        VariationAccessPlan = variationAccessPlan;
        TransmissionSourceAccessPlan = transmissionSourceAccessPlan;
        TransmissionDestinationAccessPlan = transmissionDestinationAccessPlan;
    }
}