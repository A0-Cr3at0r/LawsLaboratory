using LawsLaboratory.Application.Execution.ExecutionRequestStage;
using LawsLaboratory.Core.Formula.Element;

namespace LawsLaboratory.Application.Execution.EngineGateway.Entry;

public enum ExpressionKinds : byte
{
    Operator = 0,
    Variable = 1,
    Constant = 2,
    Symbol = 3
}

public readonly record struct ExpressionEntry(
    ExpressionKinds Kind,
    double Value);

internal sealed class GatewayEntryBuffer
{
    public int[] BoxLimite { get; set; }

    private readonly List<ExpressionEntry> _expression;

    private readonly RequestPacket[] _packets;

    internal Span<RequestPacket> Packets =>
    _packets.AsSpan();

    internal  List<ExpressionEntry> Expression { get => _expression; }

    public GatewayEntryBuffer(
                        int maxPackets,
                        int maxValueCount,
                        int maxBoxUsable,
                        CompiledExpression firstExpression)
    {
        BoxLimite = new int[maxBoxUsable];

        _expression = new List<ExpressionEntry>();

        _packets = new RequestPacket[maxPackets];

        for (int i = 0; i < maxPackets; i++)
        {
            _packets[i] =
                new RequestPacket(maxValueCount);
        }

        SetParameterExpression(
            firstExpression);
    }

    public void SetParameterExpression(
    CompiledExpression compiledExpression)
    {

        _expression.Clear();

        double variableIndex = 0;

        foreach (var element in compiledExpression.Element)
        {
            switch (element)
            {
                case ConstantElement constant:

                    _expression.Add(
                        new ExpressionEntry(
                            ExpressionKinds.Constant,
                            constant.Value));

                    break;

                case SymbolElement symbol:

                    _expression.Add(
                        new ExpressionEntry(
                            ExpressionKinds.Symbol,
                            (double)symbol.Symbol));

                    break;

                case OperatorElement operator_:

                    _expression.Add(
                        new ExpressionEntry(
                            ExpressionKinds.Operator,
                            (double)operator_.Operator));

                    break;

                case VariableElement:

                    _expression.Add(
                        new ExpressionEntry(
                            ExpressionKinds.Variable,
                            variableIndex++));

                    break;
            }
        }
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