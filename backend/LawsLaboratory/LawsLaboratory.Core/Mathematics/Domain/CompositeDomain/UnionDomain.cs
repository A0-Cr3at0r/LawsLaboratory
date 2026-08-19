// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Domain / CompositeDomain
//
// UnionDomain.cs
//
// Represents the union of multiple domains. A value belongs to the resulting
// domain when it belongs to at least one component domain.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Core.Mathematics.Domain.CompositeDomain;

public sealed class UnionDomain<T> : IDomain<T>
{
    private readonly IDomain<T>[] _domains;


    public UnionDomain(
        IEnumerable<IDomain<T>> domains)
    {
        _domains = domains.ToArray();

        if (_domains.Length == 0)
            throw new ArgumentException(
                "At least one domain is required.");
    }


    public bool Contains(T value)
    {
        foreach (var domain in _domains)
        {
            if (domain.Contains(value))
                return true;
        }

        return false;
    }
}