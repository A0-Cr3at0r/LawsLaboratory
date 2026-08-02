using LawsLaboratory.Core.Formula;

namespace LawsLaboratory.Application.FormulaCompiler.Semantic;

internal sealed class FunctionDefinition
{
    public OperatorType Operator { get; }

    public int ArgumentCount { get; }


    public FunctionDefinition(
        OperatorType op,
        int argumentCount)
    {
        Operator = op;
        ArgumentCount = argumentCount;
    }
}