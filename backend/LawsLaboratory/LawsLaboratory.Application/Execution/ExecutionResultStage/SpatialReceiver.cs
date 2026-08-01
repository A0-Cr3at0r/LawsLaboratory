namespace LawsLaboratory.Application.Execution.ExecutionResultStage;

internal sealed class SpatialReceiver<T>
{
    private readonly ExecutionResultBuffer<T> _buffer;

    private int _cellId;

    private ushort _parameterId;

    private T _result = default!;

    public int CellId => _cellId;

    public ushort ParameterId => _parameterId;

    public T Result => _result;

    public SpatialReceiver(
        ExecutionResultBuffer<T> buffer)
    {
        _buffer = buffer;
    }

    public bool Receive()
    {
        if (!_buffer.TryAcquireRead(out ResultPacket<T> packet))
        {
            return false;
        }

        _cellId = packet.CellId;
        _parameterId = packet.ParameterId;
        _result = packet.Result;

        _buffer.ReleaseRead();

        return true;
    }
}