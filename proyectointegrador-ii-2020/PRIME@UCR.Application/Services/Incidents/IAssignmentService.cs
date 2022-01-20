using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Application.Services.Incidents
{
    public interface IAssignmentService
    {
        Task<IEnumerable<UnidadDeTransporte>> GetAllTransportUnitsByMode(string mode);
        Task<IEnumerable<CoordinadorTécnicoMédico>> GetCoordinatorsAsync();
        Task<IEnumerable<EspecialistaTécnicoMédico>> GetSpecialistsAsync();
        Task<Médico> GetAssignedOriginDoctor(string code);
        Task<Médico> GetAssignedDestinationDoctor(string code);
        Task<AssignmentModel> GetAssignmentsByIncidentIdAsync(string code);
        Task AssignToIncidentAsync(string code, AssignmentModel model);
        // only assigned doctors and medical techs, or any coordinator should be authorized
        public Task<bool> IsAuthorizedToViewPatient(string code, string personId);
    }
}
