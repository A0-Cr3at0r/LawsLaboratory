
// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Formula / Program
//
// ExpressionProgram.cs
//
// Defines the kinds of instructions that can appear in a compiled expression
// program.
//
// Each kind determines how the instruction value is interpreted by the
// expression execution engine.
//
//Represents a single executable instruction in a compiled expression program.
//
// The instruction kind determines the meaning of Value:
// - Constant: numeric constant
// - Symbol: SymbolType value
// - Variable: index of the corresponding variable reference
// - Operator: OperatorType value
// -----------------------------------------------------------------------------

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

