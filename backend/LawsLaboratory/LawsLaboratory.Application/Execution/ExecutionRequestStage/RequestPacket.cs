// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Execution / ExecutionRequestStage
//
// RequestPacket.cs
//
// Represents one request prepared for the execution engine.
//
// A packet associates a source CellId with a fixed-capacity collection of
// scalar input values. Unused value slots are represented by null.
//
// The packet is a mutable reusable storage object intended to be held inside
// the preallocated GatewayEntryBuffer. Clear() resets the packet so its
// storage can be reused without allocating a new packet.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Execution.ExecutionRequestStage;

internal sealed class RequestPacket
{
    public int CellId { get; internal set; }

    public double?[] Values { get; }


    public RequestPacket(int MaxValueCount)
    {
        Values = new double?[MaxValueCount];

        CellId = -1;
    }

    public void Clear()
    {
        for (int i = 0; i < Values.Length; i++) { 
            Values[i] = null;
        }

        CellId = -1;
    }

    public void Write(
    int index,
    double value)
    {
        Values[index] = value;
    }

}