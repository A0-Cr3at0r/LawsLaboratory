using LawsLaboratory.Core.SpatialModel;
using LawsLaboratory.Core.Value;

namespace LawsLaboratory.Core.Formula.Element;

public sealed class VariableElement : ExpressionElement
{
    public int ParameterId { get; }

    public  Position RelativePosition { get; }

    public IValue Value { get; private set; }

    public VariableElement( int parameterId, Position relativePosition)
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
