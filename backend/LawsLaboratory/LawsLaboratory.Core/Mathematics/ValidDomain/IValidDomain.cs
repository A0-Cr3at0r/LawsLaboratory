namespace LawsLaboratory.Core.Mathematics.Domain;

public interface IValidDomain<T>
{
    bool IsMine(T value);
    T Discretize(T value);
}