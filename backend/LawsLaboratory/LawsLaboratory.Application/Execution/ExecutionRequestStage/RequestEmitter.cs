// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Execution / ExecutionRequestStage
//
// RequestEmitter.cs
//
// Writes a prepared spatial request into the GatewayEntryBuffer.
//
// RequestEmitter isolates the physical layout of a request in the entry
// buffer from the RequestController. It copies the values required by one
// request and associates the request with its source CellId.
//
// It performs no spatial access, traversal, or formula evaluation.
// -----------------------------------------------------------------------------

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
        _buffer.BoxLimiBoxRequestCounts[boxId] = boxLimit;
    }
}