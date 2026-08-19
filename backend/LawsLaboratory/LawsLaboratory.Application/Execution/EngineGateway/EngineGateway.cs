using LawsLaboratory.Application.Engine;
using LawsLaboratory.Application.Execution.EngineGateway.Entry;
using LawsLaboratory.Application.Execution.EngineGateway.Exit;
using LawsLaboratory.Core.Formula.Program;


namespace LawsLaboratory.Application.Execution.Gateway;

using Program = List<ExpressionInstruction>;

internal class EngineGateway
{
    public bool UseInternalMotor { private get; set; } = true;

    private Action? _notifyCalculationCompleted;

    internal GatewayEntryBuffer EntryBuffer { get; }

    internal GatewayExitBuffer ExitBuffer { get; }

    private readonly DefaultEngine _defaultEngine;

    private readonly int _boxSize;

    private readonly int _boxCount;


    public EngineGateway(
        int maxPackets,
        int maxValueCount,
        int maxBoxUsable,
         Program program)
    {

        EntryBuffer =
            new GatewayEntryBuffer(
                maxPackets,
                maxValueCount,
                maxBoxUsable,
                program);

        ExitBuffer =
            new GatewayExitBuffer(maxPackets);

        _defaultEngine =
            new DefaultEngine(EntryBuffer.Expression);

        _boxCount = EntryBuffer.BoxLimite.Length;

        if (_boxCount <= 0)
        {
            throw new InvalidOperationException(
                "The gateway must contain at least one box.");
        }

        _boxSize = maxPackets / _boxCount;

        if (_boxSize <= 0)
        {
            throw new ArgumentException(
                "maxPackets must be greater than or equal to the number of boxes.",
                nameof(maxPackets));
        }

    }


    /// <summary>
    /// Defines the callback used to notify the TaskCoordinator
    /// that all result data for the current parameter
    /// has been calculated and written into the GatewayExitBuffer.
    ///
    /// This method exists separately from the constructor
    /// because the coordinator and gateway are initialized
    /// independently by the simulation.
    /// </summary>
    public void SetNotifyCalculationCompletedCallback(
        Action callback)
    {
        ArgumentNullException.ThrowIfNull(callback);

        _notifyCalculationCompleted = callback;
    }


    /// <summary>
    /// Called by the TaskCoordinator when all request packets
    /// have been written into the GatewayEntryBuffer.
    /// </summary>
    public void StartCalculation()
    {
        if (UseInternalMotor)
        {
            ExecuteInternalCalculation();
            return;
        }

        ExecuteExternalCalculation();
    }


    /// <summary>
    /// Executes the calculation using the internal DefaultEngine.
    /// </summary>
    public void ExecuteInternalCalculation()
    {
        var packets = EntryBuffer.Packets;
        var boxLimits = EntryBuffer.BoxLimite;
        var results = ExitBuffer.Results;

        ExitBuffer.ResultReceived = 0;

        for (int boxId = 0; boxId < _boxCount; boxId++)
        {
            int boxLimit = boxLimits[boxId];

            int boxBegin = boxId * _boxSize;

            for (int localIndex = 0; localIndex < boxLimit; localIndex++)
            {
                int packetIndex = boxBegin + localIndex;

                var packet = packets[packetIndex];

                double result =
                    _defaultEngine.Evaluate(packet.Values);

                results[packetIndex] =
                    new GatewayResult
                    {
                        Id = packet.CellId,
                        Value = CreateSerializedValue(result)
                    };

                ExitBuffer.ResultReceived++;
            }
        }

        _notifyCalculationCompleted?.Invoke();
    }


    /// <summary>
    /// Executes the calculation using the external engine.
    /// Not implemented yet.
    /// </summary>
    public void ExecuteExternalCalculation()
    {
        throw new NotImplementedException();
    }


    private static SerializedValue CreateSerializedValue(
        double value)
    {
        return new SerializedValue(
            ValueKind.Scalar,
            Array.Empty<int>(),
            new[] { value });
    }
}