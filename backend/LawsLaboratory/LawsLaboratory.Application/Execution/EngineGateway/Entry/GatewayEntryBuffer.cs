using LawsLaboratory.Application.Execution.ExecutionRequestStage;
using LawsLaboratory.Core.Formula.Element;
using System.Reflection.Metadata;

namespace LawsLaboratory.Application.Execution.EngineGateway.Entry;

public enum ExpressionKinds : byte
{
    Variable,
    Constant,
    Symbol,
    Operator
}

public readonly record struct ExpressionEntry(
    ExpressionKinds Kind,
    double Value);

internal sealed class GatewayEntryBuffer
{

    private readonly List<ExpressionEntry> _expression;

    private readonly RequestPacket[] _packets;

    public GatewayEntryBuffer(
                        int maxPackets,
                        int maxValueCount,
                        ushort firstParameterId,
                        CompiledExpression firstExpression)
    {
        _expression = new List<ExpressionEntry>();

        _packets = new RequestPacket[maxPackets];

        for (int i = 0; i < maxPackets; i++)
        {
            _packets[i] =
                new RequestPacket(maxValueCount);
        }

        SetParameterExpression(
            firstParameterId,
            firstExpression);
    }

    public void SetParameterExpression(
    ushort parameterId,
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

    public void Write(int packetIndex, int valIndex, double val)
    {
        _packets[packetIndex].Write(valIndex, val);
    }

}