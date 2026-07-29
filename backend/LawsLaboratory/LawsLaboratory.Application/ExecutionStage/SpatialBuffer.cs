/*
 * ExecutionBuffer is a preallocated circular buffer designed for a
 * single-producer / single-consumer execution pipeline.
 *
 * The writer and reader operate independently using their own indices.
 * Availability flags remove the ambiguity of equal read/write indices:
 * - availableToRead indicates that committed packets are waiting.
 * - availableToWrite indicates that free packet slots are available.
 *
 * A packet becomes visible to the reader only after CommitWrite().
 * A packet becomes reusable only after ReleaseRead().
 *
 * This design guarantees that the writer and reader never access the
 * same packet simultaneously, allowing future execution on separate threads.
 */

namespace LawsLaboratory.Application.ExecutionStage;
public sealed class ExecutionBuffer
{
    private readonly SpatialPacket[] _packets;

    private int _writeIndex;
    private int _readIndex;

    private bool _availableToRead;
    private bool _availableToWrite;


    public int Capacity => _packets.Length;


    public ExecutionBuffer(
        int packetCount,
        int maxVariableCount)
    {
        _packets = new SpatialPacket[packetCount];

        for (int i = 0; i < packetCount; i++)
        {
            _packets[i] =
                new SpatialPacket(maxVariableCount);
        }

        _availableToWrite = true;
        _availableToRead = false;
    }


    public bool TryAcquireWrite(
        out SpatialPacket packet)
    {
        if (!_availableToWrite)
        {
            packet = null!;
            return false;
        }

        packet = _packets[_writeIndex];

        return true;
    }


    public void CommitWrite()
    {
        _writeIndex++;

        if (_writeIndex == Capacity)
        {
            _writeIndex = 0;
        }

        _availableToRead = true;


        if (_writeIndex == _readIndex)
        {
            _availableToWrite = false;
        }
    }


    public bool TryAcquireRead(
        out SpatialPacket packet)
    {
        if (!_availableToRead)
        {
            packet = null!;
            return false;
        }

        packet = _packets[_readIndex];

        return true;
    }


    public void ReleaseRead()
    {

        _readIndex++;

        if (_readIndex == Capacity)
        {
            _readIndex = 0;
        }

        _availableToWrite = true;


        if (_readIndex == _writeIndex)
        {
            _availableToRead = false;
        }
    }
}