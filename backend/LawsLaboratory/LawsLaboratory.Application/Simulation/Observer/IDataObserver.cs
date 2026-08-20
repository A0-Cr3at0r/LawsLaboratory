// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Observer
//
// IDataObserver.cs
//
// Defines the contract for components that consume observations emitted during
// simulation execution.
//
// Observers receive observations through Notify and are intentionally decoupled
// from the component that produces them. The interface is contravariant so that
// an observer of a broader observation type can be reused where appropriate.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.Observer;
public interface IDataObserver<in TObservation>
{
    void Notify(TObservation observation);
}