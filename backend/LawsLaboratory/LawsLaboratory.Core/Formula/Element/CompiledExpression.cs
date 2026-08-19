using LawsLaboratory.Core.Formula.Program;
using LawsLaboratory.Core.SpatialModel.Position;


namespace LawsLaboratory.Core.Formula.Element;

using Program = List<ExpressionInstruction>;


public sealed class CompiledExpression
{
    private  List<VariableReference> references;

    public Program Element { get; private set; } 

    public CompiledExpression(IReadOnlyList<ExpressionElement> elements)
    {
        Element = initExpressionProgram(elements);

        int variableCount = 0;

        foreach (ExpressionElement element in elements)
        {
            if (element is VariableElement)
            {
                variableCount++;
            }
        }

        references = new(variableCount);


        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i] is VariableElement)
            {
                VariableElement variable = (VariableElement)elements[i];

                references.Add(
                new VariableReference(
                variable.ParameterId,
                variable.RelativePosition));
            }
        }

    }


    public IReadOnlyList<VariableReference> GetVariableReferences()
    {
        return references;
    }


    public Program initExpressionProgram(IReadOnlyList<ExpressionElement> expressionElements)
    {

        Program program = new     Program ();

        double variableIndex = 0;

        foreach (var element in expressionElements)
        {
            switch (element)
            {
                case ConstantElement constant:

                    program.Add(
                        new ExpressionInstruction(
                            ExpressionKinds.Constant,
                            constant.Value));

                    break;

                case SymbolElement symbol:

                    program.Add(
                        new ExpressionInstruction(
                            ExpressionKinds.Symbol,
                            (double)symbol.Symbol));

                    break;

                case OperatorElement operator_:

                    program.Add(
                        new ExpressionInstruction(
                            ExpressionKinds.Operator,
                            (double)operator_.Operator));

                    break;

                case VariableElement:

                    program.Add(
                        new ExpressionInstruction(
                            ExpressionKinds.Variable,
                            variableIndex++));

                    break;
            }
        }

        return program;
    }

}