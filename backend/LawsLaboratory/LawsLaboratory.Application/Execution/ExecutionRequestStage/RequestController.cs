using LawsLaboratory.Application.Execution.EngineGateway.Entry;
using LawsLaboratory.Application.Simulation.Observer;
using LawsLaboratory.Application.Simulation.SpatialManagement.ReaderWriter;
using LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Application.Execution.ControllersState;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;


namespace LawsLaboratory.Application.Execution.ExecutionRequestStage;

 internal sealed class RequestController
{
    private readonly int _beginAt;

    private readonly int _boxId;

    private readonly SpatialRepository _spatialRepository;

    ITraversalCursor _cursor;

    private readonly SpatialReader _spatialReader;

    private readonly RequestEmitter _requestEmitter;

    ushort _currentParameterId;

    private SpatialAccessPlan? _currentAccessPlan;

    private ControllerState _controllerState;

    public ControllerState ControllerState { 
                        get => _controllerState; 
                                          }

    public int Count { get; private set; } = 0;


    public RequestController(
        int beginAt, 
        int boxSize,
        int boxId,
        SpatialRepository spatialRepository, 
        ObservationDispatcher observerDispatcher,
        IGrid<PlanePosition> grid, 
        int MaxVariableCount,
        GatewayEntryBuffer buffer,
        ITraversalStrategy<int> traversal)
    {
        _beginAt = beginAt;
        _boxId = boxId;
        _spatialRepository = spatialRepository;
        _spatialReader = new SpatialReader(grid, observerDispatcher, MaxVariableCount);
        _requestEmitter = new RequestEmitter(buffer);
        _cursor = traversal.CreateCursor(boxSize);
        _controllerState = ControllerState.None;
    }

    public void SetParameterId(ushort parameterId)
    {
        _currentParameterId = parameterId;
    }

    public void StartVariation()
    {
        _currentAccessPlan = _spatialRepository
                                    .GetPlan(_currentParameterId)
                                    .VariationAccessPlan;

        _controllerState = ControllerState.Running;

        Count = 0;

        RunDataEmission();
    }

    public void StartTransmission()
    {
        _currentAccessPlan = _spatialRepository
                                    .GetPlan(_currentParameterId)
                                    .TransmissionSourceAccessPlan;

        _controllerState = ControllerState.Running;

        Count = 0;

        RunDataEmission();
    }


    private void RunDataEmission()
    {
        int packetId = 0;

        do
        {
            int cellId = _beginAt + _cursor.Current;

            

            if (_spatialReader.TryRead(
                cellId,
                _currentAccessPlan!,
                _currentParameterId))
            {
                packetId = _beginAt + Count++;

                _requestEmitter.Emit(
                    packetId,
                    _spatialReader.Values,
                    _spatialReader.Count,
                    cellId);

            }

        } while (_cursor.TryAdvance());

        _cursor.Reset();

        _requestEmitter.updateBoxLimit(_boxId, Count);

        _controllerState = ControllerState.Completed;
    }


}
