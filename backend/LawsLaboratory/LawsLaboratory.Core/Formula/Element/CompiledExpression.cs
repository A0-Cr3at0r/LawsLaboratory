// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Formula
//
// CompiledExpression.cs
//
// Represents the compiled form of a formula expression.
//
// A compiled expression contains:
// - an executable expression program composed of ExpressionInstructions;
// - the spatial variable references required to resolve variable values.
//
// The expression program is consumed by the execution layer, while the
// variable references are used by the spatial planning and data preparation
// stages of the simulation pipeline.
// -----------------------------------------------------------------------------

using LawsLaboratory.Core.Formula.Program;
using LawsLaboratory.Core.SpatialModel.Position;


namespace LawsLaboratory.Core.Formula.Element;

using Program = List<ExpressionInstruction>;


public sealed class CompiledExpression
{
    private readonly List<VariableReference> _references;

    public Program Program { get; } 

    public CompiledExpression(IReadOnlyList<ExpressionElement> elements)
    {
        Program = initExpressionProgram(elements);

        int variableCount = 0;

        foreach (ExpressionElement element in elements)
        {
            if (element is VariableElement)
            {
                variableCount++;
            }
        }

        _references = new(variableCount);


        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i] is VariableElement)
            {
                VariableElement variable = (VariableElement)elements[i];

                _references.Add(
                new VariableReference(
                variable.ParameterId,
                variable.RelativePosition));
            }
        }

    }


    public IReadOnlyList<VariableReference> GetVariableReferences()
    {
        return _references;
    }


    private Program initExpressionProgram(IReadOnlyList<ExpressionElement> expressionElements)
    {

        Program program = new  Program ();

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