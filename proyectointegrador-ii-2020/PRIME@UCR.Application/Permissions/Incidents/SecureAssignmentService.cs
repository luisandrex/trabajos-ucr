using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models.Incidents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.Implementations.Incidents;

namespace PRIME_UCR.Application.Permissions.Incidents
{

    public class  SecureAssignmentService : IAssignmentService
    {
        private readonly ITransportUnitRepository _transportUnitRepository;
        private readonly ICoordinadorTécnicoMédicoRepository _coordinatorRepo;
        private readonly IEspecialistaTécnicoMédicoRepository _specialistRepo;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly IIncidentRepository _incidentRepository;
        private readonly IPrimeSecurityService _primeSecurityService;
        private readonly AssignmentService _assignmentService;
 
        public SecureAssignmentService(ITransportUnitRepository transportUnitRepository,
            ICoordinadorTécnicoMédicoRepository coordinatorRepo,
            IEspecialistaTécnicoMédicoRepository specialistRepo,
            IAssignmentRepository assignmentRepo,
            IIncidentRepository incidentRepository,
            IPrimeSecurityService primeSecurityService)
        {
            _transportUnitRepository = transportUnitRepository;
            _coordinatorRepo = coordinatorRepo;
            _specialistRepo = specialistRepo;
            _assignmentRepo = assignmentRepo;
            _incidentRepository = incidentRepository;
            _primeSecurityService = primeSecurityService;
            _assignmentService = new AssignmentService(transportUnitRepository, coordinatorRepo, specialistRepo, assignmentRepo, incidentRepository);
        }

        public async Task AssignToIncidentAsync(string code, AssignmentModel model)
        {
            await _primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanAssignIncidents });
            await _assignmentService.AssignToIncidentAsync(code, model);
        }

        public async Task<AssignmentModel> GetAssignmentsByIncidentIdAsync(string code)
        {
            return await _assignmentService.GetAssignmentsByIncidentIdAsync(code);
        }

        public async Task<IEnumerable<UnidadDeTransporte>> GetAllTransportUnitsByMode(string mode)
        {
            return await _assignmentService.GetAllTransportUnitsByMode(mode);
        }

        public async Task<IEnumerable<CoordinadorTécnicoMédico>> GetCoordinatorsAsync()
        {
            return await _assignmentService.GetCoordinatorsAsync();
        }

        public async Task<IEnumerable<EspecialistaTécnicoMédico>> GetSpecialistsAsync()
        {
            return await _assignmentService.GetSpecialistsAsync();
        }

        public async Task<Médico> GetAssignedOriginDoctor(string code)
        {
            return await _assignmentService.GetAssignedOriginDoctor(code);
        }

        public async Task<Médico> GetAssignedDestinationDoctor(string code)
        {
            return await _assignmentService.GetAssignedDestinationDoctor(code);
        }

        public async Task<bool> IsAuthorizedToViewPatient(string code, string personId)
        {
            return await _assignmentService.IsAuthorizedToViewPatient(code, personId);
        }
    }
}
