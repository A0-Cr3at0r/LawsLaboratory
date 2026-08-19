// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / SpatialManagement / ReaderWriter
//
// SpatialWriter.cs
//
// Materializes an execution result into the simulation grid.
//
// The writer supports two forms of destination:
//   - a direct (cell, parameter) destination;
//   - a precomputed SpatialAccessPlan describing multiple spatial destinations.
//
// It contains no execution or law logic. Its sole responsibility is to apply
// an already computed value to the appropriate grid location(s).
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.SpatialManagement.ReaderWriter;

using LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Core.Value;

internal sealed class SpatialWriter
{
    private readonly IGrid<PlanePosition> _grid;


    public SpatialWriter(
        IGrid<PlanePosition> grid)
    {
        _grid = grid;
    }

    public void Write(
        int cellId,
        ushort parameterId,
        IValue value)
    {
        _grid.SetCellParameterValue(
            cellId,
            parameterId,
            value);
    }


    public void Write(
        int cellId,
        SpatialAccessPlan accessPlan,
        IValue value)
    {
        for (int i = 0; i < accessPlan.Count; i++)
        {
            SpatialAccess access =
                accessPlan.GetAccess(i);

            _grid.SetCellParameterValue(
                cellId + access.CellOffset, 
                access.ParameterId, 
                value); 

        }
    }
}