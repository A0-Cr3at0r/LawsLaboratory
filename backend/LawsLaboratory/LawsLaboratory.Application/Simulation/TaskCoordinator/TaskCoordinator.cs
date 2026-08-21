// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / TaskCoordinator
//
// TaskCoordinator.cs
//
// Orchestrates the execution stages of a simulation cycle.
//
// For each simulation parameter, the coordinator:
//   1. selects the corresponding law expression;
//   2. launches the RequestControllers;
//   3. notifies the EngineGateway that the input data is ready;
//   4. waits for calculation completion;
//   5. determines the number of results actually produced;
//   6. distributes the valid result range among the ResultControllers;
//   7. launches the ResultControllers;
//   8. synchronizes the execution phases.
//
// The coordinator does not perform formula evaluation and does not directly
// access the simulation grid. Request and result controllers perform the
// spatial data operations, while the EngineGateway is responsible for
// calculation.
//
// The GatewayExitBuffer is preallocated and its capacity may exceed the
// number of results produced by a calculation. The ResultReceived value
// determines the valid result range for the current calculation. Entries
// outside this range may remain uninitialized and must not be consumed.
//
// Consequently, ResultControllers are configured according to the actual
// number of received results rather than the maximum buffer capacity. The
// coordinator distributes this valid range so that each ResultController
// operates only on a valid portion of the result data.
//
// Controllers are created once during initialization and reused across
// simulation cycles.
//
// Variation and transmission share the same execution pipeline but use
// different spatial access plans and law expressions.
//
// The TaskCoordinator does not execute calculations and has no knowledge
// of the underlying execution engine.
//
// TaskCoordinatorState represents the current synchronization stage of the
// pipeline, including calculation waits and lifecycle states such as pause,
// resume, completion, and stop.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Execution.EngineGateway.Exit;
using LawsLaboratory.Application.Execution.EngineGateway.Entry;
using LawsLaboratory.Application.Execution.ExecutionRequestStage;
using LawsLaboratory.Application.Execution.ExecutionResultStage;
using LawsLaboratory.Application.Simulation.Build.Factories;
using LawsLaboratory.Core.Laws;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.LawsRepository;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.Parameter;

namespace LawsLaboratory.Application.Simulation.TaskCoordinatorNameSpace;


internal enum TaskCoordinatorState
{
    None,

    RunningVariationRequests,

    WaitingVariationCalculation,

    RunningVariationResults,

    RunningTransmissionRequests,

    WaitingTransmissionCalculation,

    RunningTransmissionResults,

    Paused,

    Completed,

    Stopped
}

/// <summary>
/// Coordinates the execution pipeline of a simulation cycle.
///
/// The TaskCoordinator is responsible for:
/// - launching RequestControllers;
/// - notifying the gateway when input data is ready;
/// - waiting for calculus completion notification;
/// - launching ResultControllers;
/// - synchronizing execution phases.
///
/// The TaskCoordinator does not execute calculations and has no
/// knowledge of the underlying execution engine.
/// </summary>
internal sealed class TaskCoordinator
{

    private readonly GatewayEntryBuffer _gatewayEntryBuffer;

    private readonly GatewayExitBuffer _gatewayExit;

    private readonly IParameterRegistry _parameterRegistry;

    private readonly Laws _laws;


    private readonly IReadOnlyList<RequestController> _requestControllers;

    private readonly IReadOnlyList<ResultController> _resultControllers;


    private Action? _startCalculation;


    private TaskCompletionSource? _calculusCompletionSource;


    private TaskCoordinatorState _state;

    private TaskCoordinatorState _memoryState;

    public TaskCoordinatorState State => _state;

    /// <summary>
    /// Creates a new TaskCoordinator.
    ///
    /// Controllers are created once during initialization
    /// and reused for every simulation cycle.
    /// </summary>
    public TaskCoordinator(
        RequestControllerFactory requestControllerFactory,
        ResultControllerFactory resultControllerFactory,
        GatewayEntryBuffer gatewayEntryBuffer,
        GatewayExitBuffer gatewayExit,
        IParameterRegistry parameterRegistry,
        Laws laws,
        int cellCount)
    {
        ArgumentNullException.ThrowIfNull(requestControllerFactory);
        ArgumentNullException.ThrowIfNull(resultControllerFactory);
        ArgumentNullException.ThrowIfNull(gatewayEntryBuffer);
        ArgumentNullException.ThrowIfNull(gatewayExit);
        ArgumentNullException.ThrowIfNull(parameterRegistry);
        ArgumentNullException.ThrowIfNull(laws);


        _gatewayEntryBuffer =
            gatewayEntryBuffer;

        _gatewayExit =
            gatewayExit;


        _parameterRegistry =
            parameterRegistry;

        _laws =
            laws;


        _requestControllers =
            requestControllerFactory
                .CreateControllers(cellCount);


        _resultControllers =
            resultControllerFactory
                .CreateControllers(cellCount);


        _state =
            TaskCoordinatorState.None;
    }

    /// <summary>
    /// Defines the callback used to notify the gateway
    /// that all request data for the current parameter
    /// has been emitted.
    ///
    /// This method exists separately from the constructor
    /// because the coordinator and gateway are initialized
    /// independently by the simulation.
    /// </summary>
    public void SetStartCalculation(
        Action callback)
    {
        ArgumentNullException.ThrowIfNull(callback);

        _startCalculation =
            callback;
    }

    /// <summary>
    /// Called by the gateway when the current calculus
    /// has been completed.
    /// </summary>
    public void NotifyCalculusCompleted()
    {
        _calculusCompletionSource?
            .TrySetResult();
    }


    public async Task StartVariation()
    {
        _state =
            TaskCoordinatorState.RunningVariationRequests;

        for (ushort parameterId = 0;
             parameterId < _parameterRegistry.Count;
             parameterId++)
        {
            await ExecuteParameterAsync(
                parameterId,
                true);
        }

        _state =
            TaskCoordinatorState.Completed;
    }


    public async Task StartTransmission()
    {
        _state =
            TaskCoordinatorState.RunningTransmissionRequests;

        for (ushort parameterId = 0;
             parameterId < _parameterRegistry.Count;
             parameterId++)
        {
            await ExecuteParameterAsync(
                parameterId,
                false);
        }

        _state =
            TaskCoordinatorState.Completed;
    }

    /// <summary>
    /// Requests a pause after the current execution stage.
    /// </summary>
    public void Pause()
    {
        _memoryState = _state;

        _state =
            TaskCoordinatorState.Paused;
    }

    
    public void Resume()
    {
        if (_state != TaskCoordinatorState.Paused)
            return;

        _state =
            _memoryState;
    }

    
    public void Stop()
    {
        _state =
            TaskCoordinatorState.Stopped;
    }


    private void ResetCalculusCompletion()
    {
        _calculusCompletionSource =
            new TaskCompletionSource(
                TaskCreationOptions
                    .RunContinuationsAsynchronously);
    }

    /// <summary>
    /// Executes a complete parameter calculation pipeline.
    ///
    /// Pipeline:
    /// 
    /// Set expression
    ///      ↓
    /// Emit requests
    ///      ↓
    /// Notify gateway
    ///      ↓
    /// Wait calculus completion
    ///      ↓
    /// Absorb results
    /// </summary>
    private async Task ExecuteParameterAsync(
        ushort parameterId,
        bool variation)
    {
        Law law =
            _laws.GetLaw(parameterId);


        _gatewayEntryBuffer.SetExpression(
            variation
                ? law.GetVariationExpression().Program
                : law.GetTransmissionExpression().Program);


        await ExecuteRequestStageAsync(
            parameterId,
            variation);


        ResetCalculusCompletion();

        _state =
            variation
            ? TaskCoordinatorState.WaitingVariationCalculation
            : TaskCoordinatorState.WaitingTransmissionCalculation;

        _startCalculation?
            .Invoke();

        await _calculusCompletionSource!
            .Task;

        _state =
                variation
                ? TaskCoordinatorState.RunningVariationResults
                : TaskCoordinatorState.RunningTransmissionResults;


        await ExecuteResultStageAsync(
            parameterId,
            variation);
    }

    /// <summary>
    /// Executes all request controllers in parallel.
    /// </summary>
    private async Task ExecuteRequestStageAsync(
        ushort parameterId,
        bool variation)
    {
        List<Task> tasks = new(
            _requestControllers.Count);


        foreach (RequestController controller
                 in _requestControllers)
        {
            controller.SetParameterId(
                parameterId);


            tasks.Add(
                Task.Run(() =>
                {
                    if (variation)
                    {
                        controller.StartVariation();
                    }
                    else
                    {
                        controller.StartTransmission();
                    }
                }));
        }


        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Executes the result reception stage.
    /// 
    /// The number of controllers depends on the
    /// amount of results actually produced by the engine.
    /// </summary>
    private async Task ExecuteResultStageAsync(
    ushort parameterId,
    bool variation)
    {
        int resultCount =
            _gatewayExit.ResultReceived;


        if (resultCount == 0)
            return;


        int controllerCount =
            ComputeControllerCount(resultCount);


        int remaining =
            resultCount;


        List<Task> tasks = new();


        for (int i = 0; i < controllerCount; i++)
        {
            ResultController controller =
                _resultControllers[i];


            int boxSize =
                ComputeBoxSize(
                    remaining,
                    controllerCount - i);


            controller.SetParameters(
                parameterId,
                boxSize);


            tasks.Add(
                Task.Run(() =>
                {
                    if (variation)
                        controller.StartVariation();
                    else
                        controller.StartTransmission();
                }));


            remaining -= boxSize;
        }


        await Task.WhenAll(tasks);
    }


    private int ComputeControllerCount(
    int resultCount)
    {
        if (resultCount <= 0)
            return 0;


        /*
            Controller distribution heuristic:

            0 - 10 000 cells
                -> 1 controller

            10 000 - 500 000
                -> partition according to available controllers

            500 000 - 1 000 000
                -> maximum parallelism
        */


        if (resultCount <= 10_000)
        {
            return 1;
        }


        int maxControllerCount =
            _resultControllers.Count;


        if (resultCount >= 500_000)
        {
            return maxControllerCount;
        }


        int optimalCount =
            resultCount / 50_000;


        return Math.Clamp(
            optimalCount,
            1,
            maxControllerCount);
    }

    private int ComputeBoxSize(
    int remaining,
    int remainingControllers)
    {
        if (remainingControllers <= 1)
        {
            return remaining;
        }


        return remaining / remainingControllers;
    }
}