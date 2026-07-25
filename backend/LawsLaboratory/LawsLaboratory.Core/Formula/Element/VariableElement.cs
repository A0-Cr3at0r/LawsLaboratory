using LawsLaboratory.Core.SpatialModel;
using LawsLaboratory.Core.Value;

namespace LawsLaboratory.Core.Formula.Element;

public sealed class VariableElement : ExpressionElement
{
    public int ParameterId { get; }

    public RelativePosition Position { get; }

    public IValue Value { get; private set; }

    public VariableElement( int parameterId, RelativePosition position)
    {
        ParameterId = parameterId;
        Position = position;
        Value = Dead.Instance;
    }

    internal void Assign(IValue value)
    {
        Value = value;
    }
}
