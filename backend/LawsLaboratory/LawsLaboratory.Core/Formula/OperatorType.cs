namespace LawsLaboratory.Core.Formula;

public enum OperatorType : byte
{
    Add = 0,
    Subtract = 1,
    Multiply = 2,
    Divide = 3,
    Power = 4,

    Log = 5,
    Ln = 6,
    Sqrt = 7,

    Sin = 8,
    Cos = 9,

    And = 10,
    Or = 11,
    Xor = 12,
    Not = 13,

    Floor = 14,
    Ceil = 15
}
