namespace LawsLaboratory.Application.Observer;
public interface IDataObserver<in TObservation>
{
    void Notify(TObservation observation);
}