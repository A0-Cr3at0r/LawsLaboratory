namespace LawsLaboratory.Core.SpatialModel.Position;

public readonly record struct VariableReference(
    int ParameterId,
    PlanePosition RelativePosition
    );
