namespace LawsLaboratory.Application.Simulation.Observer;
public interface IDataObserver<in TObservation>
{
    void Notify(TObservation observation);
}