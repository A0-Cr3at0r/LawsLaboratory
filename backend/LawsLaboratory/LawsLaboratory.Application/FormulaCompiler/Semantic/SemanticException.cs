// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / FormulaCompiler / Semantic
//
// SemanticException.cs
//
// Represents a semantic compilation error.
//
// Used when a syntactically valid formula cannot be interpreted according
// to the vocabulary and rules of the formula domain.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Application.FormulaCompiler.Semantic;

internal sealed class SemanticException : Exception
{
    public SemanticException(
        string message)
        : base(message)
    {
    }
}