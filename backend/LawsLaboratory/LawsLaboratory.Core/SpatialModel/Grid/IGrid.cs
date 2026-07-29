using LawsLaboratory.Core.SpatialModel.Grid;

internal interface IGrid<TPosition>
    where TPosition : struct
{
    int Dimension { get; }

    int Size { get; }

    Cell GetCell(int id);

    TPosition GetPosition(int id);

    bool Contains(TPosition position);
}