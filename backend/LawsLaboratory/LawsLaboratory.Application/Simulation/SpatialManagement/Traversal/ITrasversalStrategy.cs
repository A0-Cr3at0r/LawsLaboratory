// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / SpatialManagement / Traversal
//
// ITraversalStrategy.cs
//
// Defines the factory abstraction for traversal strategies.
//
// A traversal strategy creates a cursor responsible for iterating through a
// simulation domain according to a specific traversal policy.
//
// The strategy is separated from the cursor so that traversal policies can
// be exchanged without coupling the processing logic to a concrete iteration
// mechanism.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;

internal interface ITraversalStrategy<TContext>
{
    ITraversalCursor CreateCursor(TContext context);
}