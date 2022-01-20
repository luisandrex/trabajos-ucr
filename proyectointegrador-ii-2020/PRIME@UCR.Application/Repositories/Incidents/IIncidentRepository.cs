using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Application.Repositories.Incidents
{
    public interface IIncidentRepository : IGenericRepository<Incidente, string>
    {
        Task<Incidente> GetWithDetailsAsync(string code);
        Task<Incidente> GetIncidentByDateCodeAsync(int id);
        Task<IEnumerable<IncidentListModel>> GetIncidentListModelsAsync();
        Task<Médico> GetAssignedOriginDoctor(string code);
        Task<Médico> GetAssignedDestinationDoctor(string code);
        Task<IEnumerable<OngoingIncident>> GetOngoingIncidentsAsync(Modalidad unitType, Estado state);
        Task<IEnumerable<IncidentListModel>> GetAuthorizedDoctorIncidentListModelsAsync(string id);
        Task<IEnumerable<IncidentListModel>> GetAuthorizedSpecialistIncidentListModelsAsync(string id);
        Task<IEnumerable<string>> GetAuthorizedCodesForSpecialist(string id);
        Task<IEnumerable<string>> GetAuthorizedCodesForOriginDoctor(string id);
        Task<IEnumerable<string>> GetAuthorizedCodesForDestinationDoctor(string id);
        Task<CambioIncidente> GetLastChange(string code);
        Task UpdateLastChange(CambioIncidente change);
    }
}
