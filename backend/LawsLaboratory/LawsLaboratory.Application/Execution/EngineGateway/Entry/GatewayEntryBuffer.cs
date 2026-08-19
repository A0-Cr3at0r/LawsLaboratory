using LawsLaboratory.Application.Execution.ExecutionRequestStage;
using LawsLaboratory.Core.Formula.Element;
using LawsLaboratory.Core.Formula.Program;


namespace LawsLaboratory.Application.Execution.EngineGateway.Entry;

using Program = List<ExpressionInstruction>;


internal sealed class GatewayEntryBuffer
{
    public int[] BoxLimite { get; set; }

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
        BoxLimite = new int[maxBoxUsable];

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