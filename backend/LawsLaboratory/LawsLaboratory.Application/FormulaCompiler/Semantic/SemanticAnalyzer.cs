namespace LawsLaboratory.Application.FormulaCompiler.Semantic;

using LawsLaboratory.Application.Simulation.EnvironnementRepository.Parameter;
using LawsLaboratory.Core.Formula;
using LawsLaboratory.Core.Formula.Node;

internal sealed class SemanticAnalyzer
{
    private readonly IParameterRegistry _parameterRegistry;

    private static readonly IReadOnlyDictionary<string, SymbolType>
        Symbols =
            new Dictionary<string, SymbolType>(StringComparer.OrdinalIgnoreCase)
            {
                {"pi", SymbolType.Pi},
                {"e", SymbolType.Euler}
            };


    private static readonly IReadOnlyDictionary<string, FunctionDefinition>
        Functions =
            new Dictionary<string, FunctionDefinition> (StringComparer.OrdinalIgnoreCase)
            {
                {   "sin",
                    new FunctionDefinition(
                        OperatorType.Sin,
                        1)
                },

                {  "cos",
                    new FunctionDefinition(
                        OperatorType.Cos,
                        1)
                },

                {   "sqrt",
                    new FunctionDefinition(
                        OperatorType.Sqrt,
                        1)
                },

                {   "ln",
                    new FunctionDefinition(
                        OperatorType.Ln,
                        1)
                },

                {   "log",
                    new FunctionDefinition(
                        OperatorType.Log,
                        1)
                },

                {   "floor",
                    new FunctionDefinition(
                        OperatorType.Floor,
                        1)
                },

                {   "ceil",
                    new FunctionDefinition(
                        OperatorType.Ceil,
                        1)
                }
            };


    public SemanticAnalyzer(
        IParameterRegistry parameterRegistry)
    {
        ArgumentNullException.ThrowIfNull(
            parameterRegistry);

        _parameterRegistry = parameterRegistry;
    }


    public ExpressionNode Analyze(
        ExpressionNode syntaxTree)
    {
        ArgumentNullException.ThrowIfNull(
            syntaxTree);

        return AnalyzeNode(
            syntaxTree);
    }


    private ExpressionNode AnalyzeNode(
        ExpressionNode node)
    {
        return node switch
        {
            ConstantNode constant =>
                AnalyzeConstant(constant),


            IdentifierNode identifier =>
                AnalyzeIdentifier(identifier),


            OperatorNode operation =>
                AnalyzeOperator(operation),


            FunctionCallNode function =>
                AnalyzeFunctionCall(function),


            _ =>
                throw new SemanticException(
                    $"Unsupported syntax node '{node.GetType().Name}'.")
        };
    }


    private ExpressionNode AnalyzeConstant(
        ConstantNode node)
    {
        return new ConstantNode(
            node.Value);
    }


    private ExpressionNode AnalyzeIdentifier(
        IdentifierNode node)
    {
        if (_parameterRegistry.ContainsParameter(node.Name))
        {
            return new VariableNode(
                _parameterRegistry.GetParameterId(node.Name),
                node.RelativePosition);
        }


        if (Symbols.TryGetValue(
                node.Name,
                out SymbolType symbol))
        {
            return new SymbolNode(
                symbol);
        }


        throw new SemanticException(
            $"Unknown identifier '{node.Name}'.");
    }


    private ExpressionNode AnalyzeOperator(
        OperatorNode node)
    {
        List<ExpressionNode> children =
            new(node.Children.Count);


        foreach (ExpressionNode child in node.Children)
        {
            children.Add(
                AnalyzeNode(child));
        }


        return new OperatorNode(
            node.Operator,
            children);
    }


    private ExpressionNode AnalyzeFunctionCall(
        FunctionCallNode node)
    {
        if (!Functions.TryGetValue(
                node.Name,
                out FunctionDefinition? definition))
        {
            throw new SemanticException(
                $"Unknown function '{node.Name}'.");
        }


        if (node.Arguments.Count != definition.ArgumentCount)
        {
            throw new SemanticException(
                $"Function '{node.Name}' expects " +
                $"{definition.ArgumentCount} argument(s) " +
                $"but received {node.Arguments.Count}.");
        }


        List<ExpressionNode> arguments =
            new(node.Arguments.Count);


        foreach (ExpressionNode argument in node.Arguments)
        {
            arguments.Add(
                AnalyzeNode(argument));
        }


        return new OperatorNode(
            definition.Operator,
            arguments);
    }
}
