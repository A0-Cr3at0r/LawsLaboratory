// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / GridObservation
//
// GridBufferPool.cs
//
// Provides a bounded pool of reusable byte buffers for grid observations.
// The pool guarantees that every rented buffer is large enough for a complete
// grid capture.
// -----------------------------------------------------------------------------

using System.Buffers;

namespace LawsLaboratory.Application.Simulation.GridObservation;

internal sealed class GridBufferPool
{
    private const int DefaultMaximumBuffers = 10;

    private readonly object _sync = new();

    private readonly Stack<byte[]> _availableBuffers = new();

    private readonly int _bufferSize;
    private readonly int _maximumBuffers;

    private int _createdBuffers;

    public GridBufferPool(
        int bufferSize,
        int maximumBuffers = DefaultMaximumBuffers)
    {
        if (bufferSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(bufferSize));
        }

        if (maximumBuffers <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maximumBuffers));
        }

        _bufferSize = bufferSize;
        _maximumBuffers = maximumBuffers;
    }

    public byte[] Rent()
    {
        lock (_sync)
        {
            if (_availableBuffers.Count > 0)
            {
                return _availableBuffers.Pop();
            }

            if (_createdBuffers >= _maximumBuffers)
            {
                throw new InvalidOperationException(
                    "The maximum number of grid buffers is currently in use.");
            }

            byte[] buffer =
                ArrayPool<byte>.Shared.Rent(_bufferSize);

            _createdBuffers++;

            return buffer;
        }
    }

    public void Return(byte[] buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        if (buffer.Length < _bufferSize)
        {
            throw new ArgumentException(
                "The returned buffer is smaller than the pool buffer size.",
                nameof(buffer));
        }

        lock (_sync)
        {
            _availableBuffers.Push(buffer);
        }
    }
}