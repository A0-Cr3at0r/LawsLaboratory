using LawsLaboratory.Application.FormulaCompiler.Lexer;
using LawsLaboratory.Application.FormulaCompiler.Optimization;
using LawsLaboratory.Application.FormulaCompiler.Parser;
using LawsLaboratory.Application.FormulaCompiler.Semantic;
using LawsLaboratory.Application.FormulaCompiler.CompiledGenerator;
using LawsLaboratory.Core.Formula.Element;
using LawsLaboratory.Core.Formula.Node;
using LawsLaboratory.Application.Simulation.Registry;


namespace LawsLaboratory.Core.Formula;

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