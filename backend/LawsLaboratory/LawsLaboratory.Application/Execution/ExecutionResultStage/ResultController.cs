// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Execution / ExecutionResultStage
//
// ResultController.cs
//
// Applies the results produced by the execution engine back to the simulation
// grid for a given parameter and traversal range.
//
// The controller:
//   - reads results from the GatewayExitBuffer;
//   - converts them through ResultAbsorber;
//   - determines their destination according to the execution mode;
//   - writes the resulting value into the grid through SpatialWriter.
//
// Variation writes each result to the parameter value of the cell identified
// by the result.
//
// Transmission uses the parameter's precomputed destination spatial access
// plan to write the result to the appropriate destination cells.
//
// It does not perform formula evaluation. Its responsibility is to materialize
// engine results into the simulation state.
//
// The controller reports its execution state through ControllerState.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Execution.ControllersState;
using LawsLaboratory.Application.Simulation.SpatialManagement.ReaderWriter;
using LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Application.Execution.EngineGateway.Exit;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;

namespace LawsLaboratory.Application.Execution.ExecutionResultStage;

internal sealed class ResultController
{
    private readonly int _beginAt;

    private readonly SpatialRepository _spatialRepository;

    private readonly ITraversalCursor _cursor;

    private readonly SpatialWriter _spatialWriter;

    private readonly ResultAbsorber _resultAbsorber;

    private ushort _currentParameterId;

    private SpatialAccessPlan? _currentAccessPlan;

    private bool _isVariation;

    private ControllerState _controllerState;

    public ControllerState ControllerState => _controllerState;


    public ResultController(
        int beginAt,
        int boxSize,
        SpatialRepository spatialRepository,
        IGrid<PlanePosition> grid,
        GatewayExitBuffer gatewayExit,
        ITraversalStrategy<int> traversal)
    {
        _beginAt = beginAt;
        _spatialRepository = spatialRepository;

        _spatialWriter = new SpatialWriter(grid);
        _resultAbsorber = new ResultAbsorber(gatewayExit);

        _cursor = traversal.CreateCursor(boxSize);

        _controllerState = ControllerState.None;
    }


    public void SetParameters(
        ushort parameterId, 
        int boxSize)
    {
        _currentParameterId = parameterId;

        _cursor.TraversalCount = boxSize;
    }


    public void StartVariation()
    {
        _isVariation = true;

        _controllerState = ControllerState.Running;

        RunDataReception();
    }


    public void StartTransmission()   {
        _isVariation = false;

        _currentAccessPlan = _spatialRepository
            .GetPlan(_currentParameterId)
            .TransmissionDestinationAccessPlan;

        _controllerState = ControllerState.Running;

        RunDataReception();
    }


    private void RunDataReception()
    {
        do
        {
            if (_resultAbsorber.TryAbsorb(_cursor.Current + _beginAt))
            {
                if (_isVariation)
                {
                    _spatialWriter.Write(
                        _resultAbsorber.Id,
                        _currentParameterId,
                        _resultAbsorber.Value);
                }
                else
                {
                    _spatialWriter.Write(
                        _resultAbsorber.Id,
                        _currentAccessPlan!,
                        _resultAbsorber.Value);
                }
            }

        } while (_cursor.TryAdvance());

        _cursor.Reset();

        _controllerState = ControllerState.Completed;
    }
}