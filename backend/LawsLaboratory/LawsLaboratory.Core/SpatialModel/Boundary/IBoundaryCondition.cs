namespace LawsLaboratory.Core.SpatialModel.Boundary;

public interface IBoundaryCondition<TPosition>
    where TPosition : struct
{
    TPosition Resolve(TPosition position);
}