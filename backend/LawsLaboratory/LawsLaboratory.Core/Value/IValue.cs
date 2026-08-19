// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Value
//
// IValue.cs
//
// Defines the abstraction for values manipulated by the simulation engine.
// A value can either represent a live scalar value or the Dead state.
//
// Implementations:
// - Dead
// - ScalarValue
//
// Notes:
// - Set operations may mutate the current instance or return another IValue
//   implementation when the represented state changes.
// - Clone returns an independent value, except for stateless singleton values.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.Value;

public interface IValue
{
    IValue Set(double value);

    IValue Set(IValue value);

    IValue Clone();

    double? Get();
}