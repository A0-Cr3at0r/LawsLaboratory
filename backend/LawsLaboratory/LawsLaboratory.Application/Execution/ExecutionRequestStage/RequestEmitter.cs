using LawsLaboratory.Application.Execution.EngineGateway.Entry;
using LawsLaboratory.Core.Value;

namespace LawsLaboratory.Application.Execution.ExecutionRequestStage;


internal sealed class RequestEmitter
{
    private readonly GatewayEntryBuffer _buffer;

    public RequestEmitter(
        GatewayEntryBuffer buffer)
    {
        _buffer = buffer;
    }

    public void Emit(
        int packetIndex,
        ReadOnlySpan<IValue> values,
        int count,
        int CellID)

    {
        for (int i = 0; i < count; i++)
        {
            _buffer.Write(
                packetIndex,
                i,
                (double)values[i].Get()!);
        }

        _buffer.WriteCellID(packetIndex, CellID);
    }

    public void updateBoxLimit(int boxId, int boxLimit) {
        _buffer.BoxLimite[boxId] = boxLimit;
    }
}