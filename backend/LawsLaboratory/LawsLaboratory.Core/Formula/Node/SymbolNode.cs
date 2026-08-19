// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Formula / Node
//
// SymbolNode.cs
//
// Represents a symbolic mathematical constant in a formula expression,
// such as Pi or Euler's number.
// -----------------------------------------------------------------------------
using LawsLaboratory.Core.Formula;

namespace LawsLaboratory.Core.Formula.Node;

public sealed class SymbolNode : ExpressionNode
{
    public SymbolType Symbol { get; }

    public SymbolNode(SymbolType symbol)
    {
        Symbol = symbol;
    }
}
