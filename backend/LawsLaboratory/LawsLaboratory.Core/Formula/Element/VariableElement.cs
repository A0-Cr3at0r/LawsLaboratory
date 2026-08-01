using LawsLaboratory.Core.SpatialModel;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Core.Value;

namespace LawsLaboratory.Core.Formula.Element;

public sealed class VariableElement : ExpressionElement
{
    public ushort ParameterId { get; }

    public PlanePosition RelativePosition { get; }

    public IValue Value { get; private set; }

    public VariableElement(ushort parameterId, PlanePosition relativePosition)
    {
        ParameterId = parameterId;
        RelativePosition = relativePosition;
        Value = Dead.Instance;
    }

    internal void Assign(IValue value)
    {
        Value = value;
    }
}
