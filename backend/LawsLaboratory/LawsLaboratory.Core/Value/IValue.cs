namespace LawsLaboratory.Core.Value;

public interface IValue
{
    IValue Set(double value);

    IValue Set(IValue value);

    IValue Clone();

    double? Get();
}