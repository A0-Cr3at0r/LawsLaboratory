namespace LawsLaboratory.Core.Mathematics.Domain;

public interface IDomain<T>
{
    bool Contains(T value);
}