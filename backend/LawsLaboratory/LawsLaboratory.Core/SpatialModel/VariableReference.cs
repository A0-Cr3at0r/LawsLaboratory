namespace LawsLaboratory.Core.SpatialModel.Position;

public readonly record struct VariableReference(
    ushort ParameterId,
    PlanePosition RelativePosition
    );
