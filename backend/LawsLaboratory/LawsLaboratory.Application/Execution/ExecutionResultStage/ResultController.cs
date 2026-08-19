using LawsLaboratory.Application.Execution.ControllersState;
using LawsLaboratory.Application.Simulation.SpatialManagement.Access;
using LawsLaboratory.Application.Simulation.SpatialManagement.ReaderWriter;
using LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Application.Execution.EngineGateway.Exit;

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