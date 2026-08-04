namespace  LawsLaboratory.Application.Execution.ExecutionRequestStage;

using LawsLaboratory.Core.Value;

internal sealed class SpatialEmitter
{
    private readonly ExecutionRequestBuffer _buffer;


    public SpatialEmitter(
        ExecutionRequestBuffer buffer)
    {
        _buffer = buffer;
    }


    public bool Emit(
        int cellId,
        ushort paramId,
        ReadOnlySpan<IValue> values,
        int valueCount)
    {
        if (!_buffer.TryAcquireWrite(out RequestPacket packet))
        {
            return false;
        }


        packet.Set(cellId, paramId);


        for (int i = 0; i < valueCount; i++)
        {
            packet.Values[i] = values[i];
        }


        _buffer.CommitWrite();

        return true;
    }
}