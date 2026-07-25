namespace LawsLaboratory.Core.Laws;

using LawsLaboratory.Core.Formula.Element;

public sealed class VariationRule
{
    public string Formula { get; }

    internal CompiledExpression CompiledExpression { get; }

    public VariationRule(string formula, CompiledExpression compiledExpression)
    {
        Formula = formula;
        CompiledExpression = compiledExpression;
    }

}