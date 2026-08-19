// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Domain
//
// IDomain.cs
//
// Defines an abstraction for a set of values for which domain membership
// can be evaluated.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Core.Mathematics.Domain;

public interface IDomain<T>
{
    bool Contains(T value);
}