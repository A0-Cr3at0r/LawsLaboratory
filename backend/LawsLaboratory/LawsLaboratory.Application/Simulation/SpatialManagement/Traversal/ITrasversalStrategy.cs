namespace LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;

internal interface ITraversalStrategy<TContext>
{
    ITraversalCursor CreateCursor(TContext context);
}