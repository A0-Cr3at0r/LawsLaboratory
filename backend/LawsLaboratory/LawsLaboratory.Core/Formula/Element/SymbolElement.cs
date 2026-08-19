// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Formula / Element
//
// SymbolElement.cs
//
// Represents a mathematical symbol in the intermediate formula
// representation, such as Pi or Euler's number.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.Formula.Element;

public sealed class SymbolElement : ExpressionElement
{
    public SymbolType Symbol { get; }

    public SymbolElement(SymbolType symbol)
    {
        Symbol = symbol;
    }
}
