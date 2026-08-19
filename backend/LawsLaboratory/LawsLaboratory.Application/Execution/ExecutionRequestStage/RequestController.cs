// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Execution / ExecutionRequestStage
//
// RequestController.cs
//
// Prepares the input requests sent to the execution engine for a given
// parameter and spatial traversal range.
//
// The controller:
//   - selects the spatial access plan associated with the current parameter;
//   - traverses its assigned cells;
//   - reads the required spatial values from the grid;
//   - emits the corresponding requests into the GatewayEntryBuffer.
//
// It does not perform formula evaluation. Its responsibility is limited to
// collecting and preparing the data required by the execution engine.
//
// A controller operates on one parameter and one traversal range at a time.
// Variation and transmission differ only by the spatial access plan selected
// for reading the required source values.
//
// The controller reports its execution state through ControllerState and
// updates the number of successfully emitted requests for its assigned box.
// -----------------------------------------------------------------------------

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
