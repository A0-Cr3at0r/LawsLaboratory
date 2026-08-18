namespace LawsLaboratory.Core.SpatialModel.Boundary;

using LawsLaboratory.Core.SpatialModel.Position;


internal sealed class PeriodicBoundaryCondition : IBoundaryCondition<PlanePosition>
{
    private readonly int _width;
    private readonly int _height;


    public PeriodicBoundaryCondition(int width, int height)
    {
        _width = width;
        _height = height;
    }



    public int Resolve(int cellId)
    {
        int modId = cellId % (_width * _height);

        return cellId >= 0 ? modId : modId + (_width * _height);
    }

}