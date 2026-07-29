namespace LawsLaboratory.Application.Simulation.SpatialManagement.Access;

using LawsLaboratory.Core.Formula.Element;


internal sealed class SpatialAccessCompiler
{

    public SpatialAccessPlan Compile(
        IReadOnlyList<VariableElement> variables,
        int gridWidth)
    {
        SpatialAccess[] accesses =
            new SpatialAccess[variables.Count];


        for (int i = 0; i < variables.Count; i++)
        {
            VariableElement variable = variables[i];


            int offset =
                variable.RelativePosition.Y * gridWidth
                +
                variable.RelativePosition.X;


            accesses[i] =
                new SpatialAccess(
                    variable.ParameterId,
                    offset);
        }


        return new SpatialAccessPlan(accesses);
    }
}