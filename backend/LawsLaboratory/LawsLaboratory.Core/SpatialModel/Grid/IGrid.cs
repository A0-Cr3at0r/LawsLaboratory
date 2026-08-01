namespace LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.Value;

public interface IGrid<TPosition>
    where TPosition : struct
{
    int Dimension { get; }

    int Size { get; }

    IValue GetParameterValue(int cellId, ushort parameterId);

    void SetCellParameterValue(int cellId, ushort parameterId, IValue value);

    void SetCellParameterValue(int cellId, ushort parameterId, double value);

    TPosition GetPosition(int id);

    bool Contains(TPosition position);
}