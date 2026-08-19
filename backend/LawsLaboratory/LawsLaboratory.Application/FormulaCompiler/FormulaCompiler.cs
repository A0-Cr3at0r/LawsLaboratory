// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / FormulaCompiler
//
// FormulaCompiler.cs
//
// Orchestrates the complete formula compilation pipeline.
//
// Pipeline:
//   Source → Lexing → Parsing → Semantic Analysis
//   → Optional Optimization → Compiled Generation
//
// Optimization is optional. It is primarily intended for the internal
// calculation engine and can be disabled when the compiled expression is
// intended for an external engine with its own numerical evaluation strategy.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.FormulaCompiler.Lexer;
using LawsLaboratory.Application.FormulaCompiler.Optimization;
using LawsLaboratory.Application.FormulaCompiler.Parser;
using LawsLaboratory.Application.FormulaCompiler.Semantic;
using LawsLaboratory.Application.FormulaCompiler.COmpiledGenerator;
using LawsLaboratory.Core.Formula.Element;
using LawsLaboratory.Core.Formula.Node;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.Parameter;


namespace LawsLaboratory.Application.FormulaCompiler;

public sealed class FormulaCompiler
{
    private readonly FormulaLexer _lexer;
    private readonly FormulaParser _parser;
    private readonly SemanticAnalyzer _semanticAnalyzer;
    private readonly FormulaOptimizer _optimizer;
    private readonly CompiledGenerator _compiledGenerator;

    public FormulaCompiler(IParameterRegistry parameterRegistry)
        : this(
            new FormulaLexer(),
            new FormulaParser(),
            new SemanticAnalyzer(parameterRegistry),
            new FormulaOptimizer(),
            new CompiledGenerator())
    {
    }

    internal FormulaCompiler(
        FormulaLexer lexer,
        FormulaParser parser,
        SemanticAnalyzer semanticAnalyzer,
        FormulaOptimizer optimizer,
        CompiledGenerator compiledGenerator)
    {
        _lexer = lexer;
        _parser = parser;
        _semanticAnalyzer = semanticAnalyzer;
        _optimizer = optimizer;
        _compiledGenerator = compiledGenerator;
    }

    public CompiledExpression Compile(
        string formula,
        bool optimize = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(formula);

        IReadOnlyList<FormulaToken> tokens = _lexer.Tokenize(formula);

        ExpressionNode syntaxTree = _parser.Parse(tokens);

        ExpressionNode scientificTree =
            _semanticAnalyzer.Analyze(syntaxTree);

        if (optimize)
        {
            scientificTree = _optimizer.Optimize(scientificTree);
        }

        return _compiledGenerator.Generate(scientificTree);
    }
}