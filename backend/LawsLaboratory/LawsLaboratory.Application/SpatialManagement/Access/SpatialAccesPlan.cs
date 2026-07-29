namespace LawsLaboratory.Application.SpatialManagement.Access;

internal sealed class SpatialAccessPlan
{
    private readonly SpatialAccess[] _accesses;


    public int Count => _accesses.Length;


    public SpatialAccessPlan(
        SpatialAccess[] accesses)
    {
        _accesses = accesses;
    }


    public SpatialAccess GetAccess(int index)
    {
        return _accesses[index];
    }
}