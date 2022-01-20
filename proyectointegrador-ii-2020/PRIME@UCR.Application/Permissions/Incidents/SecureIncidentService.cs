using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models.Incidents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Implementations.Incidents;
using PRIME_UCR.Application.Services.Incidents;

namespace PRIME_UCR.Application.Permissions.Incidents
{
    public class SecureIncidentService : IIncidentService
    {
        private readonly IPrimeSecurityService _primeSecurityService;
        private readonly IncidentService _incidentService;

        public SecureIncidentService(
            IIncidentRepository incidentRepository,
            IModesRepository modesRepository,
            IIncidentStateRepository statesRepository,
            ILocationRepository locationRepository,
            ITransportUnitRepository transportUnitRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IPersonaRepository personRepository,
            IAssignmentRepository assignmentRepository,
            IPrimeSecurityService primeSecurityService,
            IDocumentacionIncidenteRepository documentationRepository,
            IProfilesService profileService)
        {
            _primeSecurityService = primeSecurityService;
            _incidentService = new IncidentService(incidentRepository, 
                                                    modesRepository, 
                                                    statesRepository, 
                                                    locationRepository, 
                                                    transportUnitRepository,
                                                    medicalRecordRepository, 
                                                    personRepository, 
                                                    assignmentRepository,
                                                    documentationRepository,
                                                    profileService);
        }

        public async Task ApproveIncidentAsync(string code, string reviewerId)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanReviewIncidents });
            await _incidentService.ApproveIncidentAsync(code, reviewerId);
        }

        public async Task RejectIncidentAsync(string code, string reviewerId)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanReviewIncidents });
            await _incidentService.RejectIncidentAsync(code, reviewerId);
        }

        public async Task<IEnumerable<Incidente>> GetAllAsync()
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeIncidentsList });
            return await _incidentService.GetAllAsync();
        }

        public async Task<IncidentDetailsModel> GetIncidentDetailsAsync(string code)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeBasicDetailsOfIncidents });
            return await _incidentService.GetIncidentDetailsAsync(code);
        }

        public async Task<IncidentDetailsModel> UpdateIncidentDetailsAsync(IncidentDetailsModel model)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanEditBasicDetailsOfIncident });
            return await _incidentService.UpdateIncidentDetailsAsync(model);
        }

        public async Task<Incidente> CreateIncidentAsync(IncidentModel model, Persona person)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanCreateIncidents });
            return await _incidentService.CreateIncidentAsync(model, person);
        }

        public async Task<Incidente> GetIncidentAsync(string code)
        {
            return await _incidentService.GetIncidentAsync(code);
        }

        public async Task<Estado> GetIncidentStateByIdAsync(string code)
        {
            return await _incidentService.GetIncidentStateByIdAsync(code);
        }

        public async Task<IEnumerable<Modalidad>> GetTransportModesAsync()
        {
            return await _incidentService.GetTransportModesAsync();
        }

        public async Task<IEnumerable<IncidentListModel>> GetIncidentListModelsAsync()
        {
            return await _incidentService.GetIncidentListModelsAsync();
        }

        public async Task<IEnumerable<DocumentacionIncidente>> GetAllDocumentationByIncidentCode(string incidentCode)
        {
            return await _incidentService.GetAllDocumentationByIncidentCode(incidentCode);
        }

        public async Task<DocumentacionIncidente> InsertFeedback(string code, string feedBack)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanReviewIncidents });
            return await _incidentService.InsertFeedback(code, feedBack);
        }

        public async Task<Incidente> GetIncidentByDateCodeAsync(int id)
        {
            return await _incidentService.GetIncidentByDateCodeAsync(id);
        }

        public async Task<string> GetNextIncidentState(string code)
        {
            return await _incidentService.GetNextIncidentState(code);
        }

        public async Task<List<Tuple<string, string>>> GetPendingTasksAsync(IncidentDetailsModel model, string nexState)
        {
            return await _incidentService.GetPendingTasksAsync(model, nexState);
        }

        public List<Tuple<string, string>> GetCreatedStatePendingTasks(IncidentDetailsModel model)
        {
            return _incidentService.GetCreatedStatePendingTasks(model);
        }

        public async Task<List<Tuple<string, string>>> GetAssignedStatePendingTasks(IncidentDetailsModel model)
        {
            return await _incidentService.GetAssignedStatePendingTasks(model);
        }

        public List<Tuple<string, string>> GetApprovedStatePendingTasks(IncidentDetailsModel model)
        {
            return _incidentService.GetApprovedStatePendingTasks(model);
        }

        public async Task ChangeState(IncidentDetailsModel model, string nextState)
        {
            await _incidentService.ChangeState(model, nextState);
        }

        public async Task<bool> UpdateTransportUnit(IncidentDetailsModel model, Incidente incident)
        {
            return await _incidentService.UpdateTransportUnit(model, incident);
        }

        public async Task<List<StatesModel>> GetStatesLog(string code)
        {
            return await _incidentService.GetStatesLog(code);
        }

        public EstadoIncidente FindState(List<EstadoIncidente> statesList, Estado state)
        {
            return _incidentService.FindState(statesList, state);
        }

        public async Task<CambioIncidente> GetLastChange(string code)
        {
            return await _incidentService.GetLastChange(code);
        }

        public async Task UpdateLastChange(LastChangeModel model)
        {
            await _incidentService.UpdateLastChange(model);
        }

        public async Task<IEnumerable<IncidentListModel>> GetIncidentListModelsAsync(string id)
        {
            return await _incidentService.GetIncidentListModelsAsync(id);
        }
    }
}