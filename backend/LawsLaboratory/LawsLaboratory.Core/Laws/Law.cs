using LawsLaboratory.Core.Formula.Element;
using LawsLaboratory.Core.Mathematics.Distributions;
using LawsLaboratory.Core.Mathematics.Domain;
using LawsLaboratory.Core.SpatialModel;

namespace LawsLaboratory.Core.Laws;

public sealed class Law
{
    public int TargetParameterId { get; }

    private readonly InitializationRule _initializationRule;
    private readonly VariationRule _variationRule;
    private readonly TransmissionRule _transmissionRule;

    public Law(
        int targetParameterId,
        VariationRule variationRule,
        TransmissionRule transmissionRule,
        InitializationRule initializationRule)
    {
        TargetParameterId = targetParameterId;
        _variationRule = variationRule;
        _transmissionRule = transmissionRule;
        _initializationRule = initializationRule;
    }


    public int GetTargetCellCount()
    {
        return _initializationRule.TargetCellCount;
    }

    public IDistribution<double> GetInitializationDistribution()
    {
        return _initializationRule.Distribution;
    }

    public IValidDomain<double>? GetInitializationValidDomain()
    {
        return _initializationRule.ValidDomain;
    }

    public IReadOnlyList<VariableReference> GetVariationVariables()
    {
        return GetVariationExpression().GetVariableReferences();
    }

    public IReadOnlyList<VariableReference> GetTransmissionVariables()
    {
        return GetTransmissionExpression().GetVariableReferences();
    }

    public CompiledExpression GetVariationExpression()
    {
        return _variationRule.CompiledExpression;
    }

    public CompiledExpression GetTransmissionExpression()
    {
        return _transmissionRule.CompiledExpression;
    }

    public IReadOnlyList<Position> GetTransmissionDestinations() {
        return _transmissionRule.RelativeDestinations;
    }
}