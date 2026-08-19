// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / EnvironnementRepository / Spatial
//
// LawSpatialExecutionPlan.cs
//
// Groups the precomputed spatial access plans required to execute one Law.
//
// A law may require different spatial accesses depending on its execution
// role. The plan therefore contains:
//
//     VariationAccessPlan
//     TransmissionSourceAccessPlan
//     TransmissionDestinationAccessPlan
//
// The structure contains no execution logic. It is a compact description of
// the spatial dependencies already derived during environment construction.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;

internal readonly struct LawSpatialExecutionPlan
{
    internal SpatialAccessPlan VariationAccessPlan { get; }

    internal SpatialAccessPlan TransmissionSourceAccessPlan { get; }

    internal SpatialAccessPlan TransmissionDestinationAccessPlan { get; }


    public LawSpatialExecutionPlan(
        SpatialAccessPlan variationAccessPlan,
        SpatialAccessPlan transmissionSourceAccessPlan,
        SpatialAccessPlan transmissionDestinationAccessPlan)
    {
        VariationAccessPlan = variationAccessPlan;
        TransmissionSourceAccessPlan = transmissionSourceAccessPlan;
        TransmissionDestinationAccessPlan = transmissionDestinationAccessPlan;
    }
}