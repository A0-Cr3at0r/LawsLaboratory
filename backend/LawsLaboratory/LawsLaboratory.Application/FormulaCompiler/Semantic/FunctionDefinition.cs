
// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / FormulaCompiler / Semantic
//
// FunctionDefinition.cs
//
// Describes a function supported by the formula language.
//
// Associates a function with its semantic OperatorType representation and
// specifies the number of arguments required by that function.
//
// Used by SemanticAnalyzer to validate and resolve function calls.
// -----------------------------------------------------------------------------

using LawsLaboratory.Core.Formula;

namespace LawsLaboratory.Application.FormulaCompiler.Semantic;

internal sealed class FunctionDefinition
{
    public OperatorType Operator { get; }

    public int ArgumentCount { get; }


    public FunctionDefinition(
        OperatorType op,
        int argumentCount)
    {
        Operator = op;
        ArgumentCount = argumentCount;
    }
}