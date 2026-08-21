// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / GridObservation
//
// GridBinaryView.cs
//
// Represents a binary snapshot of the simulation grid.
// The underlying buffer is returned to the GridBufferPool when disposed.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.GridObservation;

internal sealed class GridBinaryView : IDisposable
{
    private readonly GridBufferPool _bufferPool;

    private byte[]? _buffer;

    public int Length { get; }

    internal GridBinaryView(
        GridBufferPool bufferPool,
        byte[] buffer,
        int length)
    {
        _bufferPool = bufferPool;
        _buffer = buffer;
        Length = length;
    }

    public ReadOnlyMemory<byte> Data
    {
        get
        {
            byte[]? buffer = _buffer;

            if (buffer is null)
            {
                throw new ObjectDisposedException(
                    nameof(GridBinaryView));
            }

            return buffer.AsMemory(0, Length);
        }
    }

    public void Dispose()
    {
        byte[]? buffer =
            Interlocked.Exchange(
                ref _buffer,
                null);

        if (buffer is not null)
        {
            _bufferPool.Return(buffer);
        }
    }
}