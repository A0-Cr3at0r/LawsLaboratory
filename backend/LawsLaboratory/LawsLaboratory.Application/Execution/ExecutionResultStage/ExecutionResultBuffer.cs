namespace LawsLaboratory.Application.Execution.ExecutionResultStage;

internal sealed class ExecutionResultBuffer<T>
{
    private readonly ResultPacket<T>[] _packets;

    private int _writeIndex;
    private int _readIndex;

    private bool _availableToRead;
    private bool _availableToWrite;

    public int Capacity => _packets.Length;

    public ExecutionResultBuffer(int packetCount)
    {
        _packets = new ResultPacket<T>[packetCount];

        for (int i = 0; i < packetCount; i++)
        {
            _packets[i] = new ResultPacket<T>();
        }

        _availableToWrite = true;
        _availableToRead = false;
    }

    public bool TryAcquireWrite(out ResultPacket<T> packet)
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

    public bool TryAcquireRead(out ResultPacket<T> packet)
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