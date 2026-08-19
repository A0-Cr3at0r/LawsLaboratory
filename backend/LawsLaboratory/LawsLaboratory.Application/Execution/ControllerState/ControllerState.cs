// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Execution / ControllersState
//
// ControllerState.cs
//
// Defines the execution state of a request or result controller.
//
// The state describes the local lifecycle of a controller during one
// execution stage:
//
//     None
//       ↓
//     Running
//       ↓
//     Completed
//
// The state is intentionally independent from the global TaskCoordinatorState:
// a controller reports only its own local execution status.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Application.Execution.ControllersState;

public enum ControllerState
{
    None,
    Running,
    Completed
}