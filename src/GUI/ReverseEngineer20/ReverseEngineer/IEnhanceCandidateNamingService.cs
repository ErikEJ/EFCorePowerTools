using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace ReverseEngineer20.ReverseEngineer
{
    public interface IEnhanceCandidateNamingService : ICandidateNamingService
    {
        string GenerateDbSetCandidateIdentifier(DatabaseTable originalTable);
    }
}
