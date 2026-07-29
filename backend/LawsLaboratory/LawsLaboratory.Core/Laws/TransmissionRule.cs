namespace LawsLaboratory.Core.Laws;

using LawsLaboratory.Core.Formula.Element;
using LawsLaboratory.Core.SpatialModel.Position;

public sealed class TransmissionRule
{
    public string Formula { get; }

    internal CompiledExpression CompiledExpression { get; }

    internal IReadOnlyList<PlanePosition> RelativeDestinations { get; }

    public TransmissionRule(string formula, 
                            CompiledExpression compiledExpression,
                            IReadOnlyList<PlanePosition> relativeDestinations)
    {
        Formula = formula;
        CompiledExpression = compiledExpression;
        RelativeDestinations = relativeDestinations;
    }
}