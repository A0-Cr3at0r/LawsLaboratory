// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Execution / EngineGateway / ExternalCalculation / Serializer
//
// IEngineSerializer.cs
//
// Defines the serialization boundary used to communicate calculation data
// with an external execution engine.
//
// The serializer is responsible for converting application-side execution
// data into the representation expected by an external engine and for
// interpreting the corresponding engine response.
//
// The serialization format is intentionally kept outside the execution
// pipeline so that the internal simulation model does not depend on a specific
// wire representation.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Execution.EngineGateway.ExternalCalculation.Serializer
{
    internal interface IEngineSerializer
    {
        byte[] Serialize();

        void Deserialize();
    }
}
