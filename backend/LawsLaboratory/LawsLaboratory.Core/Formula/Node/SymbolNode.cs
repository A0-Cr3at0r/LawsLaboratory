namespace LawsLaboratory.Core.Formula.Node;

public sealed class SymbolNode : ExpressionNode
{
    public SymbolType Symbol { get; }

    public SymbolNode(SymbolType symbol)
    {
        Symbol = symbol;
    }
}
