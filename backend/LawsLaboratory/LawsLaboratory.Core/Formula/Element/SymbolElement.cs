namespace LawsLaboratory.Core.Formula.Element;

public sealed class SymbolElement : ExpressionElement
{
    public SymbolType Symbol { get; }

    public SymbolElement(SymbolType symbol)
    {
        Symbol = symbol;
    }
}
