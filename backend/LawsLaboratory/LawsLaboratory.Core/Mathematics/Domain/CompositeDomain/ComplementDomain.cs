// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Domain / CompositeDomain
//
// ComplementDomain.cs
//
// Represents the complement of another domain.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Core.Mathematics.Domain.CompositeDomain;

public sealed class ComplementDomain<T> : IDomain<T>
{
    private readonly IDomain<T> _domain;


    public ComplementDomain(
        IDomain<T> domain)
    {
        _domain = domain;
    }


    public bool Contains(T value)
    {
        return !_domain.Contains(value);
    }
}