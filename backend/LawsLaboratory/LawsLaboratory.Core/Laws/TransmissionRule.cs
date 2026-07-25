namespace LawsLaboratory.Core.Laws;

using LawsLaboratory.Core.Formula.Element;

public sealed class TransmissionRule
{
    public string Formula { get; }

    internal CompiledExpression CompiledExpression { get; }

    public TransmissionRule(string formula, CompiledExpression compiledExpression)
    {
        Formula = formula;
        CompiledExpression = compiledExpression;
    }
}