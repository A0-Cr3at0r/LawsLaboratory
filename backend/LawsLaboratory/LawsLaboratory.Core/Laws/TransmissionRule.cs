namespace LawsLaboratory.Core.Laws;

using LawsLaboratory.Core.Formula.Element;
using LawsLaboratory.Core.SpatialModel;

public sealed class TransmissionRule
{
    public string Formula { get; }

    internal CompiledExpression CompiledExpression { get; }

    internal IReadOnlyList<Position> RelativeDestinations { get; }

    public TransmissionRule(string formula, 
                            CompiledExpression compiledExpression,
                            IReadOnlyList<Position> relativeDestinations)
    {
        Formula = formula;
        CompiledExpression = compiledExpression;
        RelativeDestinations = relativeDestinations;
    }
}