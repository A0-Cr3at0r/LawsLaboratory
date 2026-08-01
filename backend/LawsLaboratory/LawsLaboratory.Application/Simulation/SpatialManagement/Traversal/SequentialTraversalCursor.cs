namespace LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;

internal sealed class SequentialTraversalCursor : ITraversalCursor
{
    private readonly int _cellCount;

    public int Current { get; private set; }

    public SequentialTraversalCursor(int cellCount)
    {
        _cellCount = cellCount;

        Reset();
    }

    public bool TryAdvance()
    {
        if (Current + 1 >= _cellCount)
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