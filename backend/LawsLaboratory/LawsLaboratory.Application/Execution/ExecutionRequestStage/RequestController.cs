using LawsLaboratory.Application.Execution.EngineGateway.Entry;
using LawsLaboratory.Application.Observer;
using LawsLaboratory.Application.Simulation.SpatialManagement.Access;
using LawsLaboratory.Application.Simulation.SpatialManagement.ReaderWriter;
using LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;

namespace LawsLaboratory.Application.Execution.ExecutionRequestStage;

enum ControllerState
{
    None,
    Running,
    Completed
}
internal class RequestController
{
    private readonly int _beginAt;

    private readonly SpatialRepository _spatialRepository;

    ITraversalCursor _cursor;

    private readonly SpatialReader _spatialReader;

    private readonly RequestEmitter _requestEmitter;

    ushort _currentParameterId;

    private SpatialAccessPlan _currentAccessPlan;

    private ControllerState _controllerState;

    public ControllerState ControllerState { 
                        get => _controllerState; 
                                          }


    public RequestController(
        int beginAt, 
        int boxSize,
        SpatialRepository spatialRepository, 
        ObservationDispatcher observerDispatcher,
        IGrid<PlanePosition> grid, 
        int MaxVariableCount,
        GatewayEntryBuffer buffer,
        ITraversalStrategy<int> traversal)
    {
        _beginAt = beginAt;
        _spatialRepository = spatialRepository;
        _spatialReader = new SpatialReader(grid, observerDispatcher, MaxVariableCount);
        _requestEmitter = new RequestEmitter(buffer);
        _cursor = traversal.CreateCursor(boxSize);
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

        _controllerState = ControllerState.running;

        RunDataEmission();
    }

    public void StartTransmission()
    {
        _currentAccessPlan = _spatialRepository
                                    .GetPlan(_currentParameterId)
                                    .TransmissionSourceAccessPlan;

        _controllerState = ControllerState.running;

        RunDataEmission();
    }


    private void RunDataEmission()
    {
        do
        {
            int cellId = _beginAt + _cursor.Current;

            if (_spatialReader.TryRead(
                cellId,
                _currentAccessPlan,
                _currentParameterId))
            {
                _requestEmitter.Emit(
                    cellId,
                    _spatialReader.Values,
                    _spatialReader.Count);
            }

        } while (_cursor.TryAdvance());

        _cursor.Reset();

        _controllerState = ControllerState.Completed;
    }


}
