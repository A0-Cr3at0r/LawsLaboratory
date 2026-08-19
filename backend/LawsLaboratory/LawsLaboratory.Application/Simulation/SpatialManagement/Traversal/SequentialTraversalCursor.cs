// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / SpatialManagement / Traversal
//
// SequentialTraversalCursor.cs
//
// Implements the cursor for sequential traversal of a linearized spatial
// domain.
//
// The cursor starts at position zero and advances one position at a time
// until TraversalCount is reached.
//
// It contains only traversal state and does not perform any simulation work.
// -----------------------------------------------------------------------------

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