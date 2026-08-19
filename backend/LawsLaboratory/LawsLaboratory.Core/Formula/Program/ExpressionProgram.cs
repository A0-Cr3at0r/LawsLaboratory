namespace LawsLaboratory.Core.Formula.Program;


public enum ExpressionKinds : byte
{
    Operator = 0,
    Variable = 1,
    Constant = 2,
    Symbol = 3
}

public readonly record struct ExpressionInstruction(
    ExpressionKinds Kind,
    double Value);

