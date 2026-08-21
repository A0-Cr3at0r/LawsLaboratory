// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Build
//
// LawsBuilder.cs
//
// Builds the runtime Laws model from the declarative ModelConfiguration.
//
// For each registered parameter, the builder resolves its runtime parameter
// identifier, compiles its variation and transmission formulas, builds its
// initialization rule and constructs the corresponding Core Law.
//
// This class translates configuration data into runtime objects and does not
// implement simulation behavior itself.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Application.Simulation.Build;

using LawsLaboratory.Application.FormulaCompiler;
using LawsLaboratory.Application.Simulation.Build.InitializationBuild;
using LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.LawsRepository;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.Parameter;
using LawsLaboratory.Core.Laws;
using LawsLaboratory.Core.SpatialModel.Position;


internal sealed class LawsBuilder
{
    private readonly IParameterRegistry _registry;

    private readonly InitializationBuilder _initializationBuilder;

    private readonly FormulaCompiler _compiler;


    public LawsBuilder(
        IParameterRegistry registry)
    {
        _registry = registry;

        _compiler = new FormulaCompiler(
            registry);

        _initializationBuilder =
            new InitializationBuilder();
    }


    public Laws Build(
        ModelConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);


        Law[] laws =
            new Law[_registry.Count];


        foreach (string parameterName in _registry.ParameterNames)
        {
            ushort parameterId =
                _registry.GetParameterId(parameterName);


            LawConfiguration lawConfiguration =
                configuration.ParametersLaws[parameterName];


            laws[parameterId] =
                BuildLaw(
                    parameterId,
                    lawConfiguration);
        }


        return new Laws(laws);
    }



    private Law BuildLaw(
        ushort parameterId,
        LawConfiguration configuration)
    {
        VariationRule variation =
            new(
                configuration.VariationFormula,
                _compiler.Compile(
                    configuration.VariationFormula));


        TransmissionRule transmission =
            new(
                configuration.TransmissionFormula,
                _compiler.Compile(
                    configuration.TransmissionFormula),
                this.TransmissionDestinations(configuration.TransmissionDestinations));


        InitializationRule initialization =
            _initializationBuilder.Build(
                configuration.InitializationConfiguration);



        return new Law(
            parameterId,
            variation,
            transmission,
            initialization);
    }


    private PlanePosition[] TransmissionDestinations(
        PlanePositionConfiguration[] destinations)
    {
        PlanePosition[] positions =
            new PlanePosition[destinations.Length];

        for (int i = 0; i < destinations.Length; i++)
        {
            positions[i] =
                new PlanePosition(
                    destinations[i].X,
                    destinations[i].Y);
        }

            return positions;
    }
}