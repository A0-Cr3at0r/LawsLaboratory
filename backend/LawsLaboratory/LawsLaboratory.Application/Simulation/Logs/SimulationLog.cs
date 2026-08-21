// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Logs
//
// workflowSimulationLog.cs
//
// Represents a log message emitted during the lifecycle of a simulation
// workflow.
//
// The log communicates the current simulation state together with a
// human-readable message. When the workflow reports measurable progress,
// Current and Total provide the current and expected progress values.
//
// Current and Total are optional because not every simulation state or log
// message represents a measurable progression.
//
// workflowSimulationLog is an immutable value type intended to be passed
// between the simulation execution layer and its communication layer.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Simulation.SimulationTools;

namespace LawsLaboratory.Application.Simulation.Logs;

internal readonly record struct workflowSimulationLog
(
    SimulationState State,
    string Message,
    int? Current = null,
    int? Total = null
);