// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / SpatialModel / Grid
//
// IGrid.cs
//
// Defines the abstraction for a spatial collection of simulation cells.
//
// The grid provides access to parameter values by cell identifier while
// keeping the underlying storage and spatial representation encapsulated.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.SpatialModel.Grid;

using LawsLaboratory.Core.Value;

public interface IGrid<TPosition>
    where TPosition : struct
{
    int Width { get; }

    int Height { get; }

    int Size { get; }

    IValue GetParameterValue(
        int cellId,
        ushort parameterId);

    void SetCellParameterValue(
        int cellId,
        ushort parameterId,
        IValue value);

    void SetCellParameterValue(
        int cellId,
        ushort parameterId,
        double value);

    void CopyParameterValues(
        ushort parameterId,
        Span<double> destination);

}