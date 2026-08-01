namespace LawsLaboratory.Application.Simulation.SpatialManagement;

using LawsLaboratory.Application.Execution.ExecutionRequestStage;
using LawsLaboratory.Application.Execution.ExecutionResultStage;
using LawsLaboratory.Application.Simulation.SpatialManagement.Access;
using LawsLaboratory.Application.Simulation.SpatialManagement.ReaderWriter;
using LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;
using LawsLaboratory.Core.Value;


internal enum SpatialOperation
{
    None,
    ReadVariation,
    ReadTransmission,
    WriteTransmission
}

internal enum controllerState
{
    None,
    Pause,
    WaitingEmittion,
    WaitingReception,
    Running
}


internal sealed class SpatialController
{
    private readonly SpatialRepository _repository;

    private readonly SpatialReader _reader;
    private readonly SpatialWriter _writer;

    private readonly SpatialEmitter _emitter;
    private readonly SpatialReceiver<double> _receiver;

    private readonly ITraversalStrategy<int> _traversal;


    private SpatialOperation _operation;
    private controllerState _controllerState = controllerState.None;


    private SpatialAccessPlan? _currentReadPlan;
    private SpatialAccessPlan? _currentWritePlan;
    

    private ushort _currentParameterId;


    private readonly ITraversalCursor _readCursor;


    private bool _running;


    public SpatialController(
        SpatialRepository repository,
        SpatialReader reader,
        SpatialWriter writer,
        SpatialEmitter emitter,
        SpatialReceiver<double> receiver,
        ITraversalStrategy<int> traversal,
        int cellCount)
    {
        _repository = repository;

        _reader = reader;
        _writer = writer;

        _emitter = emitter;
        _receiver = receiver;

        _readCursor = traversal.CreateCursor(cellCount);
    }

    public void SetPhase(
        SpatialOperation operation)
    {
        _operation = operation;


        switch (operation)
        {
            case SpatialOperation.ReadVariation:

                _currentReadPlan =
                    _repository
                        .GetPlan(_currentParameterId)
                        .VariationAccessPlan;

                break;


            case SpatialOperation.ReadTransmission:

                _currentReadPlan =
                    _repository
                        .GetPlan(_currentParameterId)
                        .TransmissionSourceAccessPlan;

                break;


            case SpatialOperation.WriteTransmission:

                _currentWritePlan =
                    _repository
                        .GetPlan(_currentParameterId)
                        .TransmissionDestinationAccessPlan;

                break;

            default:
                break;
        }
    }


    public void SetCurrentParameter(
        ushort parameterId)
    {
        _currentParameterId = parameterId;
    }


    public void Reset()
    {
        _controllerState = controllerState.None;

        _readCursor.Reset();
    }


    public void Start()
    {   
        _controllerState = controllerState.Running;
        _running = true;


        while (_running)
        {
            ExecuteCurrentOperation();
        }
    }


    public void Stop()
    {
        _controllerState = controllerState.Pause;
        _running = false;
    }


    private void ExecuteCurrentOperation()
    {   
        
        switch (_operation)
        {
            case SpatialOperation.ReadVariation:

                ExecuteRead();

                break;


            case SpatialOperation.ReadTransmission:

                ExecuteRead();

                break;


            case SpatialOperation.WriteTransmission:

                ExecuteWrite();

                break;
        }
    }


    private void ExecuteRead()
    {
        if (_currentReadPlan is null)
            return;

        IValue[] values =
            _reader.Read(
                _readCursor.Current,
                _currentReadPlan);

        _controllerState = controllerState.WaitingEmittion;

        bool emitted =
            _emitter.Emit(
                _readCursor.Current,
                _currentParameterId,
                values,
                _currentReadPlan.Count);

        if (!emitted)
        {
            return;
        }

        _controllerState = controllerState.Running;

        if (!_readCursor.TryAdvance())
        {
            Stop();
            return;
        }

    }


    private void ExecuteWrite()
    {
        if (_currentWritePlan is null)
            return;

        _controllerState = controllerState.WaitingReception;

        if (!_receiver.Receive())
        {
            return;
        }

        _controllerState = controllerState.Running;

        _writer.Write(
            _receiver.CellId,
            _currentWritePlan,
            _receiver.Result);
    }

}