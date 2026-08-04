//using LawsLaboratory.Core.Value;

//Future evolution:
//IValue currently represents scalar values and Dead.
//It may later evolve to support generic mathematical objects
//(vectors, matrices, tensors, etc.) without changing the Cell API.

namespace LawsLaboratory.Core.Value
{
    public interface IValue
    {
        IValue Set(double value);

        double? Get();

    }
}