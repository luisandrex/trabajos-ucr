using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Application.Dtos;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Repositories;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Exceptions.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;
using System.ComponentModel.DataAnnotations;
using PRIME_UCR.Application.Permissions.Incidents;

namespace PRIME_UCR.Application.Implementations.Incidents
{
    internal class IncidentService : IIncidentService
    {
        private readonly IIncidentRepository _incidentRepository;
        private readonly IModesRepository _modesRepository;
        private readonly IIncidentStateRepository _statesRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ITransportUnitRepository _transportUnitRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IPersonaRepository _personRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IDocumentacionIncidenteRepository _documentationRepository;
        private readonly IProfilesService _profileService;

        public IncidentService(
            IIncidentRepository incidentRepository,
            IModesRepository modesRepository,
            IIncidentStateRepository statesRepository,
            ILocationRepository locationRepository,
            ITransportUnitRepository transportUnitRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IPersonaRepository personRepository,
            IAssignmentRepository assignmentRepository,
            IDocumentacionIncidenteRepository documentationRepository,
            IProfilesService profileService)
        {
            _incidentRepository = incidentRepository;
            _modesRepository = modesRepository;
            _statesRepository = statesRepository;
            _locationRepository = locationRepository;
            _transportUnitRepository = transportUnitRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _personRepository = personRepository;
            _assignmentRepository = assignmentRepository;
            _documentationRepository = documentationRepository;
            _profileService = profileService;
        }

        public async Task<Incidente> GetIncidentAsync(string code)
        {
            return await _incidentRepository.GetByKeyAsync(code);
        }

        public async Task<IEnumerable<Modalidad>> GetTransportModesAsync()
        {
            return await _modesRepository.GetAllAsync();
        }

        /*
         * Function: Create a new incident and adds it to the databse
         * @param: IncidentModel that contains all details in incident. And a Persona
         * that will be the administrator of that incident.
         * @return: The incident just created.
         */

        public async Task<Incidente> GetIncidentByDateCodeAsync(int id) {

            return await _incidentRepository.GetIncidentByDateCodeAsync(id);

        }


        public async Task<Incidente> CreateIncidentAsync(IncidentModel model, Persona person)
        {
            if (model.EstimatedDateOfTransfer == null)
            {
                throw new ArgumentNullException("model.EstimatedDateOfTransfer");
            }

            if (person.Cédula == null)
            {
                throw new ArgumentNullException("person.Cedula");
            }

            var entity = new Incidente
            {
                Modalidad = model.Mode.Tipo,
                CedulaAdmin = person.Cédula,
                Cita = new Cita()
            };

            entity.Cita.FechaHoraEstimada = (DateTime)model.EstimatedDateOfTransfer;

            // insert before adding state to get auto generated code from DB
            await _incidentRepository.InsertAsync(entity);

            var state = new EstadoIncidente
            {
                NombreEstado = IncidentStates.InCreationProcess.Nombre,
                CodigoIncidente = entity.Codigo,
                FechaHora = DateTime.Now,
                AprobadoPor = person.Cédula,
                Activo = true
            };

            await _statesRepository.AddState(state);

            return entity;
        }

        /*
         * Function: Gets all specific details in an incident
         * @param: incident code
         * @return: IncidentDetailsModel with all the details attributes
         */
        public async Task<IncidentDetailsModel> GetIncidentDetailsAsync(string code)
        {
            var incident = await _incidentRepository.GetWithDetailsAsync(code);
            if (incident != null)
            {
                var transportUnit = await _transportUnitRepository.GetTransporUnitByIncidentIdAsync(incident.Codigo);
                var reviewer = await _personRepository.GetByKeyPersonaAsync(incident.CedulaRevisor);
                var state = await _statesRepository.GetCurrentStateByIncidentId(incident.Codigo);
                var documentacionIncidente = await _documentationRepository.GetAllDocumentationByIncidentCode(incident.Codigo);
                List<DocumentacionIncidente> lista = documentacionIncidente.ToList();
                lista = lista.OrderBy(i => i.Id).ToList();
                DocumentacionIncidente documentacion = lista.FirstOrDefault();
                var medicalRecord =
                    incident.Cita.Expediente;
                var model = new IncidentDetailsModel
                {
                    Code = incident.Codigo,
                    Mode = incident.Modalidad,
                    CurrentState = state.Nombre,
                    Completed = incident.IsCompleted(),
                    Modifiable = incident.IsModifiable(state),
                    RegistrationDate = incident.Cita.FechaHoraCreacion,
                    EstimatedDateOfTransfer = incident.Cita.FechaHoraEstimada,
                    AdminId = incident.CedulaAdmin,
                    Origin = incident.Origen,
                    Destination = incident.Destino,
                    AppointmentId = incident.CodigoCita,
                    TransportUnitId = transportUnit?.Matricula,
                    TransportUnit = transportUnit,
                    MedicalRecord = medicalRecord,
                    Reviewer = reviewer,
                    FeedBack = documentacion
                };


                return model;
            }

            return null;
        }

        /*
         * Function: Updates to database the modified info of incident
         * @param: IncidentDetailsModel that will contain all the new info to be added into the database
         * for the update.
         * @return: the just added incidentListModel.
         */
        public async Task<IncidentDetailsModel> UpdateIncidentDetailsAsync(IncidentDetailsModel model)
        {
            var incident = await _incidentRepository.GetByKeyAsync(model.Code);

            if (model.Origin is CentroUbicacion origin
                && model.Destination is CentroUbicacion destination)
            {
                // check for medical center duplicates
                if (origin.CentroMedicoId == destination.CentroMedicoId)
                    throw new DuplicateMedicalCenterException();

                // check for doctor assignment duplicates
                if (origin.CedulaMedico == destination.CedulaMedico)
                    throw new DuplicateAssignedDoctorException();
            }

            // update origin
            var modified = await UpdateOrigin(model, incident);

            // update destination
            modified = modified || await UpdateDestination(model, incident);

            // update transport unit
            modified = modified || await UpdateTransportUnit(model, incident);

            if (modified) // write to database if the incident was modified
                await _incidentRepository.UpdateAsync(incident);

            await UpdateCompletedState(model, incident);

            return await GetIncidentDetailsAsync(incident.Codigo);
        }

        // updates the state of the incident to completed if necessary
        public async Task UpdateCompletedState(IncidentDetailsModel model, Incidente incident)
        {
            if (!model.Completed && incident.IsCompleted()) // if it was just completed but wasn't previously
            {
                var incidentState = new EstadoIncidente
                {
                    CodigoIncidente = incident.Codigo,
                    NombreEstado = IncidentStates.Created.Nombre,
                    Activo = true,
                    FechaHora = DateTime.Now,
                    AprobadoPor = model.Reviewer.Cédula
                };
                await _statesRepository.AddState(incidentState);
            }
        }

        // updates transport unit if it needs to be updated.
        // returns bool representing weather the incident was modified by this method
        public async Task<bool> UpdateTransportUnit(IncidentDetailsModel model, Incidente incident)
        {
            if (model.TransportUnit != null)
            {
                if (incident.MatriculaTrans == null || incident.MatriculaTrans != model.TransportUnit.Matricula)
                {
                    model.TransportUnit = await _transportUnitRepository.GetByKeyAsync(model.TransportUnit.Matricula);
                    incident.MatriculaTrans = model.TransportUnit.Matricula;
                    return true;
                }
            }

            return false;
        }

        // updates destination if it needs to be updated.
        // returns bool representing weather the incident was modified by this method
        public async Task<bool> UpdateDestination(IncidentDetailsModel model, Incidente incident)
        {
            if (model.Destination != null)
            {
                if (incident.IdDestino == null || incident.IdDestino != model.Destination.Id)
                {
                    var destination = await _locationRepository.InsertAsync(model.Destination);
                    incident.IdDestino = destination.Id;
                    incident.Destino = destination;
                    return true;
                }
            }

            return false;
        }

        // updates origin if it needs to be updated.
        // returns bool representing weather the incident was modified by this method
        public async Task<bool> UpdateOrigin(IncidentDetailsModel model, Incidente incident)
        {
            if (model.Origin != null)
            {
                if (incident.IdOrigen == null || incident.IdOrigen != model.Origin.Id)
                {
                    var origin = await _locationRepository.InsertAsync(model.Origin);
                    incident.IdOrigen = origin.Id;
                    incident.Origen = origin;
                    return true;
                }
            }

            return false;
        }

        /*
         * Function: Will return all incidents in a list
         * @return: IEnumerable<Incidente>
         */
        public async Task<IEnumerable<Incidente>> GetAllAsync()
        {
            return await _incidentRepository.GetAllAsync();
        }

        /*
         * Function: Returns an IEnumerable with all the incidents in the system, each one is a DTO with information to display
         * */
        public async Task<IEnumerable<IncidentListModel>> GetIncidentListModelsAsync()
        {
            return await _incidentRepository.GetIncidentListModelsAsync();
        }

        public async Task<IEnumerable<DocumentacionIncidente>> GetAllDocumentationByIncidentCode(string incidentCode)
        {
            return await _documentationRepository.GetAllDocumentationByIncidentCode(incidentCode);
        }

        public async Task<DocumentacionIncidente> InsertFeedback(string code, string feedBack)
        {
            DocumentacionIncidente newFeedBack = new DocumentacionIncidente
            {
                CodigoIncidente = code,
                RazonDeRechazo = feedBack
            };
            return await _documentationRepository.InsertAsync(newFeedBack);       
        }


        /*
         * Function: Returns authorized-to-see incidents for a specific user
         * @Params: The id (Cedula) of the registered user
         * @Return: An IEnumerable with the incidents the user has permission to see
         * @Story ID: PIG01IIC20-712
         * */
        public async Task<IEnumerable<IncidentListModel>> GetIncidentListModelsAsync(string id)
        {
            bool isAdministrator = await _profileService.IsAdministratorAsync(id);
            bool isCoordinator = await _profileService.IsCoordinatorAsync(id);
            if (isAdministrator || isCoordinator)
            {
                // A Control Center Administrator or a Coordinator can see all incidents registered in the system
                return await GetIncidentListModelsAsync();
            }
            else
            {
                bool isDoctor = await _profileService.IsDoctorAsync(id);
                bool isTechnicalSpecialist = await _profileService.IsTechnicalSpecialistAsync(id);
                IEnumerable<IncidentListModel> authorizedIncidentsList = Enumerable.Empty<IncidentListModel>();
                if (isDoctor)
                {
                    // A doctor can see the incidents where he or she is assigned either at the origin or at the destination
                    var DoctorIncidentsList = await _incidentRepository.GetAuthorizedDoctorIncidentListModelsAsync(id);
                    authorizedIncidentsList = (DoctorIncidentsList ?? Enumerable.Empty<IncidentListModel>()).Concat(authorizedIncidentsList ?? Enumerable.Empty<IncidentListModel>());
                }
                if (isTechnicalSpecialist)
                {
                    // A technical specialist can see the incidents he or she is assigned to
                    var SpecialistIncidentsList = await _incidentRepository.GetAuthorizedSpecialistIncidentListModelsAsync(id);
                    authorizedIncidentsList = (SpecialistIncidentsList ?? Enumerable.Empty<IncidentListModel>()).Concat(authorizedIncidentsList ?? Enumerable.Empty<IncidentListModel>());
                }
                return authorizedIncidentsList
                    .GroupBy(i => i.Codigo)  // Group all incidents by their code
                    .Select(g => g.First()); // Return the first one of each group, to display unique incidents only (a user can be a doctor and a specialist at the same time)
            }
        }

        /*
        * Function: Will mark the respective incident as "Approved"
        * @Params: The incident´s code.
        *          The reviewer´s Id.
        * @Return: Void
        * */
        public async Task ApproveIncidentAsync(string code, string reviewerId)
        {
            var incident = await _incidentRepository.GetByKeyAsync(code);
            if (incident == null)
            {
                throw new ArgumentException(
                    $"Invalid incident id {code}.");
            }

            var currentState = await _statesRepository.GetCurrentStateByIncidentId(code);
            if (currentState.Nombre != IncidentStates.Created.Nombre
                && currentState.Nombre != IncidentStates.Rejected.Nombre)
            {
                throw new ApplicationException("Cannot approve incident that is not in the created or rejected state.");
            }

            await _statesRepository.AddState(new EstadoIncidente
            {
                CodigoIncidente = code,
                NombreEstado = IncidentStates.Approved.Nombre,
                Activo = true,
                FechaHora = DateTime.Now,
                AprobadoPor = reviewerId
            });

            incident.CedulaRevisor = reviewerId;
            await _incidentRepository.UpdateAsync(incident);
        }

        /*
        * Function: Will mark the respective incident as "Rejected"
        * @Params: The incident´s code.
        *          The reviewer´s Id.
        * @Return: Void
        * */
        public async Task RejectIncidentAsync(string code, string reviewerId)
        {
            var incident = await _incidentRepository.GetByKeyAsync(code);
            if (incident == null)
            {
                throw new ArgumentException(
                    $"Invalid incident id {code}.");
            }

            var currentState = await _statesRepository.GetCurrentStateByIncidentId(code);
            if (currentState.Nombre != IncidentStates.Created.Nombre)
            {
                throw new ApplicationException("Cannot reject incident that is not in the created state.");
            }

            await _statesRepository.AddState(new EstadoIncidente
            {
                CodigoIncidente = code,
                NombreEstado = IncidentStates.Rejected.Nombre,
                Activo = true,
                FechaHora = DateTime.Now,
                AprobadoPor = reviewerId
            });

            incident.CedulaRevisor = reviewerId;
            await _incidentRepository.UpdateAsync(incident);
        }

        public async Task<string> GetNextIncidentState(string code)
        {
            var currentState = await _statesRepository.GetCurrentStateByIncidentId(code);
            var nextState = "";
            for (var index = 0; index < IncidentStates.IncidentStatesList.Count - 1; ++index)
            {
                if(currentState.Nombre == IncidentStates.IncidentStatesList[index].Nombre)
                {
                    nextState = IncidentStates.IncidentStatesList[index + 1].Nombre;
                    break;
                }
            }
            if(nextState == IncidentStates.Rejected.Nombre)
            {
                nextState = IncidentStates.Approved.Nombre;
            }
            return nextState;
        }

        /*
        * Function: Will find the pending tasks in order to change incident's state. Will redirect to the specific method to do so.
        * @Params: A DTO with the incident's current state.
        *          A string with the next incident's state.
        * @Return: A list with all pending tasks needed to advace to next state.
        * */
        public async Task<List<Tuple<string, string>>> GetPendingTasksAsync(IncidentDetailsModel model, string nextState)
        {
            List<Tuple<string, string>> pendingTasks = new List<Tuple<string, string>>();
            if(nextState == IncidentStates.Created.Nombre)
            {
                pendingTasks = GetCreatedStatePendingTasks(model);
            }
            else if (nextState == IncidentStates.Approved.Nombre)
            {
                pendingTasks = GetApprovedStatePendingTasks(model);
            }
            else if (nextState == IncidentStates.Assigned.Nombre)
            {
                pendingTasks = await GetAssignedStatePendingTasks(model);
            }
            return pendingTasks;
        }

        /*
         * Function: Checks for pending tasks needed to advance to "Created" state. Such tasks are: Select Orign, Select Destination, Select patient
         * @Param: A DTO with the incident's current state
         * @Return: A list with all pending tasks needed to advace to "Created" state.
         * */

        public List<Tuple<string, string>> GetCreatedStatePendingTasks(IncidentDetailsModel model)
        {
            List<Tuple<string, string>> pendingTasks = new List<Tuple<string, string>>();
            if(model.Origin == null)
            {
                pendingTasks.Add(Tuple.Create("Seleccionar origen", "Origin"));
            }
            if(model.Destination == null)
            {
                pendingTasks.Add(Tuple.Create("Seleccionar destino", "Destination"));
            }
            if(model.MedicalRecord == null)
            {
                pendingTasks.Add(Tuple.Create("Agregar paciente", "Patient"));
            }
            return pendingTasks;
        }

        /*
       * Function: Checks for pending tasks needed to advance to "Assigned" state. Such tasks are: Select TransportUnit, Select Coordinator, Select Team Members
       * @Param: A DTO with the incident's current state
       * @Return: A list with all pending tasks needed to advace to "Assigned" state.
       * */

        public async Task<List<Tuple<string, string>>> GetAssignedStatePendingTasks(IncidentDetailsModel model)
        {
            List<Tuple<string, string>> pendingTasks = new List<Tuple<string, string>>();
            var incident = await _incidentRepository.GetByKeyAsync(model.Code);
            if(incident.MatriculaTrans == null)
            {
                pendingTasks.Add(Tuple.Create("Seleccionar unidad de transporte", "Assignment"));
            }
            if (incident.CedulaTecnicoCoordinador == null)
            {
                pendingTasks.Add(Tuple.Create("Seleccionar coordinador", "Assignment"));
            }
            List<EspecialistaTécnicoMédico> teamMembers = (await _assignmentRepository.GetAssignmentsByIncidentIdAsync(incident.Codigo)).ToList();
            if (teamMembers.Count <= 0)
            {
                pendingTasks.Add(Tuple.Create("Seleccionar técnicos médicos", "Assignment"));
            }
            return pendingTasks;
        }
        /*
        * Function: Checks for pending tasks needed to advance to "Approved" state. The only task is to wait for revision.
        * @Param: A DTO with the incident's current state
        * @Return: A list with all pending tasks needed to advace to "Approved" state.
        * */
        public List<Tuple<string, string>> GetApprovedStatePendingTasks(IncidentDetailsModel model)
        {
            List<Tuple<string, string>> pendingTasks = new List<Tuple<string, string>>();
            if (model.CurrentState == IncidentStates.Created.Nombre)
            {
                pendingTasks.Add(Tuple.Create("Esperando revisión", "Info"));
            }
            else   //When its rejected.
            {
                pendingTasks.Add(Tuple.Create("Esperando una nueva revisión", "Info"));

            }
            return pendingTasks;
        }

        public async Task ChangeState(IncidentDetailsModel model, string nextState)
        {
            var incident = await _incidentRepository.GetByKeyAsync(model.Code);
            if (incident == null)
            {
                throw new ArgumentException(
                    $"Invalid incident id {model.Code}.");
            }

            await _statesRepository.AddState(new EstadoIncidente
            {
                CodigoIncidente = model.Code,
                NombreEstado = nextState,
                Activo = true,
                FechaHora = DateTime.Now,
                AprobadoPor = model.Reviewer.Cédula
            });
            await _incidentRepository.UpdateAsync(incident);
        }

        /*
         * Function:     This method will search for the current state of an incident
         *
         * Param:       Code -> Incident ID
         * Returns:     The current state of the incident given.
         */
        public async Task<Estado> GetIncidentStateByIdAsync(string code)
        {
            return await _statesRepository.GetCurrentStateByIncidentId(code);
        }

        /*
         * Function:    This method will search for an specific state. The purpose of this method is to help 
         *              the GetStatesLog method to save the states in order.
         * Param:       StatesList -> The states log of an specific incident
         *              state -> An specific state, the one we will look for in the states list, to grab all the info
         *              about it (date, approved by).
         * Returns:     That specific state of states list.
         */
        public EstadoIncidente FindState(List<EstadoIncidente> statesList, Estado state)
        {
            for (var index = 0; index < statesList.Count; ++index)
            {
                if(statesList[index].NombreEstado == state.Nombre)
                {
                    return statesList[index];
                }
            }
            return null;
        }

        /*
         * Function:    This method will search for all the states that an incident has passed. The states are not needed
         *              in the return list because this method will ensure to set that list in order.   
         * Param:       Code -> Incident ID
         * Returns:     A list OF State models of: Date when the incident reached the state, id of coordinator
         *              that aproved that change of state and the name of the state itself.
         */
        public async Task<List<StatesModel>> GetStatesLog(string code)
        {
            List<StatesModel> log = new List<StatesModel>();
            List<EstadoIncidente> statesList = (await _statesRepository.GetIncidentStatesByIncidentId(code)).ToList();
            foreach (var state in IncidentStates.IncidentStatesList)
            {
                var stateInOrder = FindState(statesList, state);
                if (stateInOrder != null)   //When an incident is not rejected, this can be null in states after rejected.
                {
                    log.Add( 
                        new StatesModel
                        {
                            NombreEstado = stateInOrder.NombreEstado,
                            AprobadoPor = stateInOrder.AprobadoPor,
                            FechaHora = stateInOrder.FechaHora
                        }
                    );
                    if (stateInOrder.Activo == true)    //To avoid iterating into pending states
                        break;
                }
            }
            return log;
        }

        public async Task<CambioIncidente> GetLastChange(string code)
        {
            return await _incidentRepository.GetLastChange(code);
        }

        public async Task UpdateLastChange(LastChangeModel model)
        {
            CambioIncidente change = new CambioIncidente
            {
                CedFuncionario = model.Responsable.Cédula,
                CodigoIncidente = model.CodigoIncidente,
                FechaHora = model.FechaHora,
                UltimoCambio = model.UltimoCambio
            };
            await _incidentRepository.UpdateLastChange(change);
        }
    }
}
