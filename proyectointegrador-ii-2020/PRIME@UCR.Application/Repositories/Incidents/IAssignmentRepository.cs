using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Application.Repositories.Incidents
{
    public interface IAssignmentRepository
    {
        Task<IEnumerable<EspecialistaTécnicoMédico>> GetAssignmentsByIncidentIdAsync(string code);
        Task AssignToIncident(string code, IEnumerable<EspecialistaTécnicoMédico> specialistId);
        public Task ClearTeamMembers(string code);
    }
}