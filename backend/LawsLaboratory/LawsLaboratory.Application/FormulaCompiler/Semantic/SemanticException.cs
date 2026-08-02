namespace LawsLaboratory.Application.FormulaCompiler.Semantic;

internal sealed class SemanticException : Exception
{
    public SemanticException(
        string message)
        : base(message)
    {
    }
}