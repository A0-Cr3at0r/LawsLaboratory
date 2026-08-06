using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Core.Value;

namespace LawsLaboratory.Core.Formula.Element;

public sealed class CompiledExpression
{
    private readonly IReadOnlyList<ExpressionElement> _elements;
    private readonly int[] _variableElementIndexes;

    public IReadOnlyList<ExpressionElement> Element  => _elements;

    public CompiledExpression(IReadOnlyList<ExpressionElement> elements)
    {
        _elements = elements;

        int variableCount = 0;

        foreach (ExpressionElement element in elements)
        {
            if (element is VariableElement)
            {
                variableCount++;
            }
        }

        _variableElementIndexes = new int[variableCount];

        int current = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i] is VariableElement)
            {
                _variableElementIndexes[current++] = i;
            }
        }
    }


    public IReadOnlyList<VariableReference> GetVariableReferences()
    {
        List<VariableReference> references = new(_variableElementIndexes.Length);

        foreach (int index in _variableElementIndexes)
        {
            VariableElement variable = (VariableElement)_elements[index];

            references.Add(
                new VariableReference(
                variable.ParameterId,
                variable.RelativePosition));
        }

        return references;
    }

    public CompiledExpression CreateAssignedExpression(IReadOnlyList<IValue> values)
    {
        if (values.Count != _variableElementIndexes.Length)
        {
            throw new ArgumentException(
                "The number of values does not match the number of variables.");
        }

        List<ExpressionElement> clonedElements = new(_elements);

        int valueIndex = 0;

        foreach (int elementIndex in _variableElementIndexes)
        {
            VariableElement original = (VariableElement)clonedElements[elementIndex];

            VariableElement assigned = new(
                original.ParameterId,
                original.RelativePosition);

            assigned.Assign(values[valueIndex++]);

            clonedElements[elementIndex] = assigned;
        }

        return new CompiledExpression(clonedElements);
    }

}