// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Domain / CompositeDomain
//
// IntersectionDomain.cs
//
// Represents the intersection of multiple domains. A value belongs to the
// resulting domain only when it belongs to every component domain.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Core.Mathematics.Domain.CompositeDomain;

public sealed class IntersectionDomain<T> : IDomain<T>
{
    private readonly IDomain<T>[] _domains;


    public IntersectionDomain(
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
            if (!domain.Contains(value))
                return false;
        }

        return true;
    }
}