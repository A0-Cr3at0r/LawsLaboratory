namespace LawsLaboratory.Core.Simulation.SpatialManagement.Traversal;

internal sealed class SequentialTraversal : ITraversalStrategy<int>
{
    public IEnumerable<int> Traverse(int cellCount)
    {
        for (int cellId = 0; cellId < cellCount; cellId++)
        {
            yield return cellId;
        }
    }
}