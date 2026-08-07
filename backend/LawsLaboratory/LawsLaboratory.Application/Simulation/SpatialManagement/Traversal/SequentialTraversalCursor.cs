namespace LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;

internal sealed class SequentialTraversalCursor : ITraversalCursor
{

    public int Current { get; private set; }

    public int TraversalCount { get; set; }

    public SequentialTraversalCursor(int cellCount)
    {
        TraversalCount = cellCount;

        Reset();
    }

    public bool TryAdvance()
    {
        if (Current + 1 >= TraversalCount)
        {
            return false;
        }

        Current++;

        return true;
    }

    public void Reset()
    {
        Current = 0;
    }
}