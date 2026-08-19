// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / SpatialManagement / Traversal
//
// SequentialTraversal.cs
//
// Provides the sequential traversal strategy.
//
// Creates SequentialTraversalCursor instances that iterate through a
// linearized simulation domain in increasing cell order.
//
// This is the basic traversal strategy and serves as the simplest concrete
// implementation of ITraversalStrategy.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;

internal sealed class SequentialTraversal : ITraversalStrategy<int>
{
    public ITraversalCursor CreateCursor(int cellCount)
    {
        return new SequentialTraversalCursor(cellCount);
    }
}