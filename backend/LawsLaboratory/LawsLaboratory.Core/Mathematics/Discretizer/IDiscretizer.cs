namespace LawsLaboratory.Core.Mathematics.Discretizer;

public interface IDiscretizer<TInput, TOutput>
{
    TOutput Discretize(TInput value);
}