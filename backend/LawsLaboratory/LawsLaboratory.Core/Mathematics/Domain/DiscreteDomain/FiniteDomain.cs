// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Domain / DiscreteDomain
//
// FiniteDomain.cs
//
// Represents a finite collection whose elements are currently addressed
// through their integer indices.
//
// This index-based representation is temporary and may be replaced by a
// domain representation specific to the underlying value type.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Core.Mathematics.Domain.DiscreteDomain;

public sealed class FiniteDomain<T> : IDomain<int>
{
    private readonly T[] _values;
    private readonly int _size;

    public FiniteDomain(IEnumerable<T> values)
    {
        _values = values.ToArray();
        _size = _values.Length;

        if (_size == 0)
            throw new ArgumentException("Domain cannot be empty.");
    }

    public bool Contains(int index)
    {
        return index >= 0 && index < _size;
    }

    public T GetValue(int index)
    {
        if (!Contains(index))
            throw new ArgumentOutOfRangeException(nameof(index));

        return _values[index];
    }
}