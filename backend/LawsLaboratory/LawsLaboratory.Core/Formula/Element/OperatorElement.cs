namespace LawsLaboratory.Core.Formula.Element;

public sealed class OperatorElement : ExpressionElement
{
    public OperatorType Operator { get; }

    public OperatorElement(OperatorType op)
    {
        Operator = op;
    }
}