using System.ComponentModel.DataAnnotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.Permissions.Incidents;
using PRIME_UCR.Application.Services.UserAdministration;

namespace PRIME_UCR.Application.Implementations.Incidents
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ITransportUnitRepository _transportUnitRepository;
        private readonly ICoordinadorTécnicoMédicoRepository _coordinatorRepo;
        private readonly IEspecialistaTécnicoMédicoRepository _specialistRepo;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly IIncidentRepository _incidentRepository;

        public AssignmentService(ITransportUnitRepository transportUnitRepository,
            ICoordinadorTécnicoMédicoRepository coordinatorRepo,
            IEspecialistaTécnicoMédicoRepository specialistRepo,
            IAssignmentRepository assignmentRepo,
            IIncidentRepository incidentRepository)
        {
            _transportUnitRepository = transportUnitRepository;
            _coordinatorRepo = coordinatorRepo;
            _specialistRepo = specialistRepo;
            _assignmentRepo = assignmentRepo;
            _incidentRepository = incidentRepository;
        }

        public async Task<IEnumerable<UnidadDeTransporte>> GetAllTransportUnitsByMode(string mode)
        {
            return await _transportUnitRepository.GetAllTransporUnitsByMode(mode);
        }

        public async Task<IEnumerable<CoordinadorTécnicoMédico>> GetCoordinatorsAsync()
        {
            return await _coordinatorRepo.GetAllAsync();
        }

        public async Task<IEnumerable<EspecialistaTécnicoMédico>> GetSpecialistsAsync()
        {
            return await _specialistRepo.GetAllAsync();
        }

        public async Task<Médico> GetAssignedOriginDoctor(string code)
        {
            return await _incidentRepository.GetAssignedOriginDoctor(code);
        }

        public async Task<Médico> GetAssignedDestinationDoctor(string code)
        {
            return await _incidentRepository.GetAssignedDestinationDoctor(code);
        }

        public async Task<AssignmentModel> GetAssignmentsByIncidentIdAsync(string code)
        {
            var incident = await _incidentRepository.GetByKeyAsync(code);
            if (incident == null)
            {
                throw new ArgumentException("Invalid incident code");
            }

            var coordinator = await _coordinatorRepo.GetByKeyAsync(incident.CedulaTecnicoCoordinador);
            var transportUnit = await _transportUnitRepository.GetByKeyAsync(incident.MatriculaTrans);
            var specialists = await _assignmentRepo.GetAssignmentsByIncidentIdAsync(code);

            return new AssignmentModel
            {
                TransportUnit = transportUnit,
                Coordinator = coordinator,
                TeamMembers = specialists.ToList()
            };
        }

        public async Task AssignToIncidentAsync(string code, AssignmentModel model)
        {
            var incident = await _incidentRepository.GetByKeyAsync(code);
            incident.CedulaTecnicoCoordinador = model.Coordinator?.Cédula;
            incident.MatriculaTrans = model.TransportUnit?.Matricula;

            await _incidentRepository.UpdateAsync(incident);
            await _assignmentRepo.ClearTeamMembers(code);
            if (model.TeamMembers.Count > 0)
                await _assignmentRepo.AssignToIncident(code, model.TeamMembers);
        }

        // checks if the personId belongs to a person assigned to this incident or if this is a coordinator
        public async Task<bool> IsAuthorizedToViewPatient(string code, string personId)
        {
            // creates a list with every authorized person
            var authorizedPeople = new List<Persona>();
            bool hasPermission = false;
            var originDoctor = await _incidentRepository.GetAssignedOriginDoctor(code);
            if (originDoctor != null)
            {
                authorizedPeople.Add(originDoctor);
            }

            var destinationDoctor = await _incidentRepository.GetAssignedDestinationDoctor(code);
            if (destinationDoctor != null)
            {
                authorizedPeople.Add(destinationDoctor);
            }

            var assignmentModel = await this.GetAssignmentsByIncidentIdAsync(code);
            var medicalTechs = assignmentModel.TeamMembers;
            foreach (var m in medicalTechs)
            {
                if(m != null)
                {
                    authorizedPeople.Add(m);
                }
            }

            hasPermission = await _coordinatorRepo.GetByKeyAsync(personId) != null;
            if (authorizedPeople.Count > 0 && !hasPermission) {
                // checks if the personId is authorized
                hasPermission = authorizedPeople.Exists(p => p.Cédula == personId);
            }
            return hasPermission;
        }
    }
}
