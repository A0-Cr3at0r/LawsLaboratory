namespace  LawsLaboratory.Application.ExecutionStage;


using LawsLaboratory.Core.Value;

internal sealed class SpatialEmitter
{
    private readonly ExecutionBuffer _buffer;


    public SpatialEmitter(
        ExecutionBuffer buffer)
    {
        _buffer = buffer;
    }


    public bool Emit(
        int cellId,
        ushort paramId,
        IValue[] values,
        int valueCount)
    {
        if (!_buffer.TryAcquireWrite(out SpatialPacket packet))
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