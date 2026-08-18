namespace LawsLaboratory.Core.SpatialModel.Boundary;

public interface IBoundaryCondition<TPosition>
    where TPosition : struct
{
    int Resolve(int position);
}