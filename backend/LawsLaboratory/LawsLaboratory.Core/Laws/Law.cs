using LawsLaboratory.Core.Formula.Element;
using LawsLaboratory.Core.Mathematics.Distributions;
using LawsLaboratory.Core.Mathematics.Domain;
using LawsLaboratory.Core.SpatialModel.Position;
using System.Numerics;

namespace LawsLaboratory.Core.Laws;

public sealed class Law
{
    public ushort TargetParameterId { get; }

    private readonly InitializationRule _initializationRule;
    private readonly VariationRule _variationRule;
    private readonly TransmissionRule _transmissionRule;

    public Law(
        ushort targetParameterId,
        VariationRule variationRule,
        TransmissionRule transmissionRule,
        InitializationRule initializationRule)
    {
        TargetParameterId = targetParameterId;
        _variationRule = variationRule;
        _transmissionRule = transmissionRule;
        _initializationRule = initializationRule;
    }


    public int? GetTargetCellCount()
    {
        return _initializationRule.TargetCellCount;
    }

    public IDistribution<double> GetInitializationValueDistribution()
    {
        return _initializationRule.ValueDistribution;
    }
    public IDistribution<Vector2>? GetInitializationSpaceDistribution()
    {
        return _initializationRule.SpaceDistribution;
    }

    public IDomain<Vector2>? GetInitializationSpaceDomain()
    {
        return _initializationRule.SpaceDomain;
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

    public IReadOnlyList<PlanePosition> GetTransmissionDestinations() {
        return _transmissionRule.RelativeDestinations;
    }
}