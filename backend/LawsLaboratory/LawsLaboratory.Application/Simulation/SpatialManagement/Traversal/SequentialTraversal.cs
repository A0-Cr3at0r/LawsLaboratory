namespace LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;

internal sealed class SequentialTraversal : ITraversalStrategy<int>
{
    public ITraversalCursor CreateCursor(int cellCount)
    {
        return new SequentialTraversalCursor(cellCount);
    }
}