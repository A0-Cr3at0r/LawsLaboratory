// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Formula / Element
//
// OperatorElement.cs
//
// Represents an operator in the intermediate formula representation.
//
// The operator determines the operation that will be encoded into the
// executable expression program during compilation.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.Formula.Element;

public sealed class OperatorElement : ExpressionElement
{
    public OperatorType Operator { get; }

    public OperatorElement(OperatorType op)
    {
        Operator = op;
    }
}