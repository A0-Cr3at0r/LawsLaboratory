// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Execution / EngineGateway / Entry
//
// GatewayEntryBuffer.cs
//
// Preallocated input buffer shared between the execution request stage and
// the EngineGateway.
//
// The buffer contains:
//   - the compiled expression/program to execute;
//   - a fixed-capacity array of reusable RequestPacket instances;
//   - the number of usable requests assigned to each controller box.
//
// The buffer is designed for repeated simulation execution with minimal
// allocation. Request controllers write their prepared requests into it,
// after which the gateway can expose the complete batch to the execution
// engine.
//
// The buffer therefore forms the application-side boundary between request
// preparation and engine execution.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Execution.ExecutionRequestStage;
using LawsLaboratory.Core.Formula.Program;


namespace LawsLaboratory.Application.Execution.EngineGateway.Entry;

using Program = List<ExpressionInstruction>;


internal sealed class GatewayEntryBuffer
{
    public int[] BoxLimiBoxRequestCounts{ get; set; }

    private readonly RequestPacket[] _packets;

    internal Span<RequestPacket> Packets =>
    _packets.AsSpan();

    internal Program Expression { get; private set; }

    public GatewayEntryBuffer(
                        int maxPackets,
                        int maxValueCount,
                        int maxBoxUsable,
                        Program program)
    {
        BoxLimiBoxRequestCounts = new int[maxBoxUsable];

        Expression = program;

        _packets = new RequestPacket[maxPackets];

        for (int i = 0; i < maxPackets; i++)
        {
            _packets[i] =
                new RequestPacket(maxValueCount);
        }
    }

    public void SetExpression(Program program)
    {
        Expression = program;
    }


    public void Clear(int packetIndex)
    {
        _packets[packetIndex].Clear();
    }

    public void Write(int packetIndex, double[] val)
    {
        for (int i = 0; i < val.Length; i++) {
            Write(packetIndex, i, val[i]);
        }
    }

    public void WriteCellID(int packetIndex, int CellID)
    {
        _packets[packetIndex].CellId =  CellID;
    }

    public void Write(int packetIndex, int valIndex, double val)
    {
        _packets[packetIndex].Write(valIndex, val);
    }

}