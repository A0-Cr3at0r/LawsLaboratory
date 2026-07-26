namespace LawsLaboratory.Core.SpatialModel;

public readonly record struct VariableReference(
    int ParameterId,
    Position RelativePosition
    );
