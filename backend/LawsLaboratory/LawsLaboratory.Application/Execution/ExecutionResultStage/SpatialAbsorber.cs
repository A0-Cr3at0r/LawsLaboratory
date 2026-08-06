namespace LawsLaboratory.Application.Execution.ExecutionResultStage;

internal sealed class SpatialAbsorber<T>
{
    private readonly ExecutionResultBuffer<T> _buffer;

    private int _cellId;

    private ushort _parameterId;

    private T _result = default!;

    public int CellId => _cellId;

    public ushort ParameterId => _parameterId;

    public T Result => _result;

    public SpatialAbsorber(
        ExecutionResultBuffer<T> buffer)
    {
        _buffer = buffer;
    }

    public bool Absorb()
    {
        if (!_buffer.TryAcquireRead(out ResultPacket<T> packet))
        {
            return false;
        }

        _cellId = packet.CellId;
        _result = packet.Result;

        _buffer.ReleaseRead();

        return true;
    }
}