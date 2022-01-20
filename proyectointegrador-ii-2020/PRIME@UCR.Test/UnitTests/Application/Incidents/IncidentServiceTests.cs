using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using PRIME_UCR.Application.Implementations.Incidents;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;
using Xunit;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Permissions.Incidents;

namespace PRIME_UCR.Test.UnitTests.Application.Incidents
{
    public class IncidentServiceTests
    {
        [Fact]
        public async Task CreateIncidentAsyncThrowsNullException()
        {
            var service = new SecureIncidentService(
                null,
                null, null, null, null, null, null, null, new AuthorizationMock().Object, null, null);

             DateTime date = new DateTime(2069, 11, 25);

             var person = new Persona { Cédula = "123" };
             var personCedulaNull = new Persona { };
             var model = new IncidentModel { EstimatedDateOfTransfer = date };
             var modelDateNull = new IncidentModel { };

             // assert
             await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateIncidentAsync(modelDateNull, person));
             await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateIncidentAsync(model, personCedulaNull));
        }

         [Fact]
         public async Task CreateIncidentAsyncReturnsValid()
         {
             var mockRepoIncident = new Mock<IIncidentRepository>();
             var mockRepoState = new Mock<IIncidentStateRepository>();

            var service = new SecureIncidentService(
                mockRepoIncident.Object,
                null, mockRepoState.Object, null, null, null, null, null, new AuthorizationMock().Object, null, null);

             DateTime date = new DateTime(2069, 11, 25);
             var mode = new Modalidad { Tipo = "Accion" };
             var person = new Persona { Cédula = "123" };
             var model = new IncidentModel { EstimatedDateOfTransfer = date, Mode = mode };

             var result = await service.CreateIncidentAsync(model, person);

             Assert.Equal(result.Modalidad, model.Mode.Tipo);
             Assert.Equal(result.CedulaAdmin, person.Cédula);
             Assert.Equal(result.Cita.FechaHoraEstimada, model.EstimatedDateOfTransfer);
         }

        [Fact]
        public async Task ApproveIncidentAsyncThrowsNullException()
        {
            var mockIncident = new Mock<IIncidentRepository>();

            mockIncident
                .Setup(p => p.GetByKeyAsync(String.Empty))
                .Returns(Task.FromResult<Incidente>(null));

            var service = new SecureIncidentService(
                mockIncident.Object,
                null, null, null, null, null, null, null, new AuthorizationMock().Object, null, null);

            await Assert.ThrowsAsync<ArgumentException>(() => service.ApproveIncidentAsync("",""));

        }

        [Fact]
        public async Task ApproveIncidentAsyncThrowsApplicationException()
        {
            var mockIncident = new Mock<IIncidentRepository>();
            var mockState = new Mock<IIncidentStateRepository>();


            var incident = new Incidente { Codigo = "1312"};
            var state = new Estado { Nombre = "Aprobado" };

            mockIncident
                .Setup(p => p.GetByKeyAsync(String.Empty))
                .Returns(Task.FromResult<Incidente>(incident));
            mockState
                .Setup(p => p.GetCurrentStateByIncidentId(String.Empty))
                .Returns(Task.FromResult<Estado>(state));

            var service = new SecureIncidentService(
                mockIncident.Object,
                null, mockState.Object, null, null, null, null, null, new AuthorizationMock().Object, null, null);

            await Assert.ThrowsAsync<ApplicationException>(() => service.ApproveIncidentAsync("",""));
        }

        [Fact]
        public async Task ApproveIncidentAsyncCompletesTask()
        {
            var mockIncident = new Mock<IIncidentRepository>();
            var mockState = new Mock<IIncidentStateRepository>();


            var incident = new Incidente { Codigo = "1312"};
            var state = new Estado { Nombre = "Rechazado" };

            mockIncident
                .Setup(p => p.GetByKeyAsync(String.Empty))
                .Returns(Task.FromResult<Incidente>(incident));
            mockState
                .Setup(p => p.GetCurrentStateByIncidentId(String.Empty))
                .Returns(Task.FromResult<Estado>(state));

            var service = new SecureIncidentService(
                mockIncident.Object,
                null, mockState.Object, null, null, null, null, null, new AuthorizationMock().Object, null, null);

            await service.ApproveIncidentAsync("","");
        }

        [Fact]
        public async Task GetAllAsyncReturnsEmptyList()
        {
            // arrange
            var mockRepo = new Mock<IIncidentRepository>();
            var data = new List<Incidente>();

            mockRepo
                .Setup(p => p.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Incidente>>(data));

            var service = new SecureIncidentService(
                mockRepo.Object,
                null, null, null, null, null, null, null, new AuthorizationMock().Object, null, null);

            // act
            var result = await service.GetAllAsync();

            // assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsyncReturnsValidList()
        {
            // arrange
            var mockRepo = new Mock<IIncidentRepository>();
            var data = new List<Incidente>
            {
                new Incidente {Codigo = "codigo1"},
                new Incidente {Codigo = "codigo2"},
                new Incidente {Codigo = "codigo3"},
                new Incidente {Codigo = "codigo4"},
                new Incidente {Codigo = "codigo5"},
                new Incidente {Codigo = "codigo6"},
            };

            mockRepo
                .Setup(p => p.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Incidente>>(data));

            var service = new SecureIncidentService(
                mockRepo.Object,
                null, null, null, null, null, null, null, new AuthorizationMock().Object, null, null);

            // act
            var result = await service.GetAllAsync();

            // assert
            Assert.Collection(result,
                                  incidente => Assert.Equal("codigo1", incidente.Codigo),
                                  incidente => Assert.Equal("codigo2", incidente.Codigo),
                                  incidente => Assert.Equal("codigo3", incidente.Codigo),
                                  incidente => Assert.Equal("codigo4", incidente.Codigo),
                                  incidente => Assert.Equal("codigo5", incidente.Codigo),
                                  incidente => Assert.Equal("codigo6", incidente.Codigo)
                              );
        }

        [Fact]
        public async Task GetIncidentAsyncReturnsNull()
        {
            // arrange
            var mockRepo = new Mock<IIncidentRepository>();
            Incidente data = null;

            mockRepo
                .Setup(p => p.GetByKeyAsync("código inválido"))
                .Returns(Task.FromResult(data));

            var service = new SecureIncidentService(
                mockRepo.Object,
                null, null, null, null, null, null, null, new AuthorizationMock().Object, null, null);

            // act
            var result = await service.GetIncidentAsync("código inválido");

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetIncidentAsyncReturnsValid()
        {
            // arrange
            var mockRepo = new Mock<IIncidentRepository>();
            var data = new Incidente{ Codigo = "código válido"};

            mockRepo
                .Setup(p => p.GetByKeyAsync("código válido"))
                .Returns(Task.FromResult(data));

            var service = new SecureIncidentService(
                mockRepo.Object,
                null, null, null, null, null, null, null, new AuthorizationMock().Object, null, null);

            // act
            var result = await service.GetIncidentAsync("código válido");

            // assert
            Assert.NotNull(result);
            Assert.Equal("código válido", result.Codigo);
        }

        [Fact]
        public async Task GetIncidentDetailsAsyncReturnsValid()
        {
            /*If the service receives valid entries it should run flawlessly.
             */
            var _MockIncidentRepository = new Mock<IIncidentRepository>();
            var _MockTransportUnitRepository = new Mock<ITransportUnitRepository>();
            var _MockPersonRepository = new Mock<IPersonaRepository>();
            var _MockStateRepository = new Mock<IIncidentStateRepository>();
            var _MockDocumentationRepository = new Mock<IDocumentacionIncidenteRepository>();
            IEnumerable<DocumentacionIncidente> _listadocu = new List<DocumentacionIncidente>();

            string CodeToTest = "codigoTotest";
            Cita CitaToTest = new Cita
            {
                Expediente = new Expediente { CedulaPaciente = "123", CedulaMedicoDuenno = "1234" },
                FechaHoraCreacion = DateTime.Today,
                FechaHoraEstimada = DateTime.Today,
            };
            Estado StateToTest = new Estado
            {
                Nombre = "estadoValido"
            };
            Incidente IncidentToTest = new Incidente
            {
                Codigo = CodeToTest,
                Cita = CitaToTest,
                CedulaRevisor = "cedulaValida",
                Modalidad = "modalityToTest",
                CedulaAdmin = "cedulaValida",
                CodigoCita = 1
            };
            UnidadDeTransporte TransportUnitToTest = new UnidadDeTransporte
            {
                Matricula = "validString"
            };
            Persona ReviewerToTest = new Persona
            {
                Cédula = "cedulaValida"
            };
            _MockIncidentRepository
                .Setup(p => p.GetWithDetailsAsync(CodeToTest))
                .Returns(Task.FromResult(IncidentToTest));
            _MockTransportUnitRepository
                .Setup(p => p.GetTransporUnitByIncidentIdAsync(IncidentToTest.Codigo))
                .Returns(Task.FromResult(TransportUnitToTest));
            _MockPersonRepository
                .Setup(p => p.GetByKeyPersonaAsync(IncidentToTest.CedulaRevisor))
                .Returns(Task.FromResult(ReviewerToTest));
            _MockStateRepository
                .Setup(p => p.GetCurrentStateByIncidentId(IncidentToTest.Codigo))
                .Returns(Task.FromResult(StateToTest));
            _MockDocumentationRepository.
                Setup(p => p.GetAllDocumentationByIncidentCode(IncidentToTest.Codigo))
                .Returns(Task.FromResult(_listadocu));
            var incidentServiceToTest = new SecureIncidentService(_MockIncidentRepository.Object, null, _MockStateRepository.Object, null, _MockTransportUnitRepository.Object, null, _MockPersonRepository.Object, null, new AuthorizationMock().Object, _MockDocumentationRepository.Object, null);

            IncidentDetailsModel result = await incidentServiceToTest.GetIncidentDetailsAsync(CodeToTest);
            Assert.True
                (
                    result.Code == IncidentToTest.Codigo
                    && result.Mode == IncidentToTest.Modalidad
                    && result.CurrentState == StateToTest.Nombre
                    && result.RegistrationDate == CitaToTest.FechaHoraCreacion
                    && result.EstimatedDateOfTransfer == CitaToTest.FechaHoraEstimada
                    && result.AdminId == IncidentToTest.CedulaAdmin
                    && result.AppointmentId == IncidentToTest.CodigoCita
                    && result.TransportUnitId == TransportUnitToTest.Matricula
                    && result.MedicalRecord == CitaToTest.Expediente
                    && result.Reviewer.Cédula == ReviewerToTest.Cédula
                );
        }

        [Fact]
        public async Task GetIncidentDetailsAsyncReturnsNull()
        {
            /*If the service receives an invalid code , it should return a null
             */
            var _MockIncidentRepository = new Mock<IIncidentRepository>();
            string CodeToTest = "codigoTotest";
            _MockIncidentRepository
               .Setup(p => p.GetWithDetailsAsync(CodeToTest))
               .Returns(Task.FromResult<Incidente>(null));
            var incidentServiceToTest = new SecureIncidentService(_MockIncidentRepository.Object, null, null, null, null, null, null, null, new AuthorizationMock().Object, null, null);
            IncidentDetailsModel result = await incidentServiceToTest.GetIncidentDetailsAsync(CodeToTest);
            Assert.Null(result);
        }

        [Fact]
        public async Task RejectIncidentAsyncReturnsValid()
        {
            /*If the service receives valid entries it should run flawlessly.
             */
            var _MockIncidentRepository = new Mock<IIncidentRepository>();
            var _MockStateRepository = new Mock<IIncidentStateRepository>();
            string code = "CodeToTest";
            string reviewerId = "ReviewerIDToTest";
            Incidente IncidentToTest = new Incidente
            {
                Codigo = code,
                CedulaRevisor = "cedulaValida",
                Modalidad = "modalityToTest",
                CedulaAdmin = "cedulaValida",
                CodigoCita = 1
            };
            Estado StateToTest = new Estado
            {
                Nombre = "Creado"
            };
            _MockIncidentRepository
                .Setup(p => p.GetByKeyAsync(code))
                .Returns(Task.FromResult(IncidentToTest));
            _MockStateRepository
                .Setup(p => p.GetCurrentStateByIncidentId(code))
                .Returns(Task.FromResult(StateToTest));
            var incidentServiceToTest = new SecureIncidentService(_MockIncidentRepository.Object, null, _MockStateRepository.Object, null, null, null, null, null, new AuthorizationMock().Object, null, null);

            await incidentServiceToTest.RejectIncidentAsync(code, reviewerId);
        }

        [Fact]
        public async Task RejectIncidentAsyncInvalidIncident()
        {
            var _MockIncidentRepository = new Mock<IIncidentRepository>();
            string CodeToTest = "CodeToTest";
            _MockIncidentRepository
               .Setup(p => p.GetByKeyAsync(CodeToTest))
               .Returns(Task.FromResult<Incidente>(null));
            var incidentServiceToTest = new SecureIncidentService(_MockIncidentRepository.Object, null, null, null, null, null, null, null, new AuthorizationMock().Object, null, null);
            await Assert.ThrowsAsync<ArgumentException>(() => incidentServiceToTest.RejectIncidentAsync(CodeToTest, ""));
        }

        [Fact]
        public async Task RejectIncidentAsyncInvalidState()
        {
            var _MockIncidentRepository = new Mock<IIncidentRepository>();
            var _MockStateRepository = new Mock<IIncidentStateRepository>();
            string CodeToTest = "CodeToTest";
            Estado StateToTest = new Estado
            {
                Nombre = "DiferenteACreado"
            };
            Incidente IncidentToTest = new Incidente
            {
                Codigo = CodeToTest,
                CedulaRevisor = "cedulaValida",
                Modalidad = "modalityToTest",
                CedulaAdmin = "cedulaValida",
                CodigoCita = 1
            };
            _MockIncidentRepository
               .Setup(p => p.GetByKeyAsync(CodeToTest))
               .Returns(Task.FromResult(IncidentToTest));
            _MockStateRepository
                .Setup(p => p.GetCurrentStateByIncidentId(IncidentToTest.Codigo))
                .Returns(Task.FromResult(StateToTest));
            var incidentServiceToTest = new SecureIncidentService(_MockIncidentRepository.Object, null, _MockStateRepository.Object, null, null, null, null, null, new AuthorizationMock().Object, null, null);
            await Assert.ThrowsAsync<ApplicationException>(() => incidentServiceToTest.RejectIncidentAsync(CodeToTest, ""));
        }

        [Fact]
        public async Task GetTransportModesAsyncReturnsNonEmptyList()
        {
            // arrange
            var _MockIncidentRepository = new Mock<IIncidentRepository>();
            var _MockModesRepository = new Mock<IModesRepository>();
            var ter = new Modalidad { Tipo = "Terrestre" };
            var mar = new Modalidad { Tipo = "Marítimo" };
            var aer = new Modalidad { Tipo = "Aéreo" };
            List<Modalidad> expected = new List<Modalidad> { ter, mar, aer, ter};

            _MockModesRepository
                .Setup(p => p.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Modalidad>>(expected));

            var service = new SecureIncidentService(
                _MockIncidentRepository.Object,
                _MockModesRepository.Object, null, null, null, null, null, null, new AuthorizationMock().Object, null, null);

            // act
            var result = (await service.GetTransportModesAsync()).ToList();

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetTransportModesAsyncReturnsEmptyList()
        {
            // arrange
            var _MockIncidentRepository = new Mock<IIncidentRepository>();
            var _MockModesRepository = new Mock<IModesRepository>();

            List<Modalidad> expected = new List<Modalidad>();

            _MockModesRepository
                .Setup(p => p.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Modalidad>>(expected));

            var service = new SecureIncidentService(
                _MockIncidentRepository.Object,
                _MockModesRepository.Object, null, null, null, null, null, null, new AuthorizationMock().Object, null, null);

            // act
            var result = (await service.GetTransportModesAsync()).ToList();

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetIncidentListModelsAsyncReturnsNonEmptyList()
        {
            // arrange
            var _MockIncidentRepository = new Mock<IIncidentRepository>();
            var incident1 = new IncidentListModel { Codigo = "123abc" };
            var incident2 = new IncidentListModel { Codigo = "456def" };
            var incident3 = new IncidentListModel { Codigo = "789ghi" };

            List<IncidentListModel> expected = new List<IncidentListModel> { incident1, incident2, incident3};

            _MockIncidentRepository
                .Setup(p => p.GetIncidentListModelsAsync())
                .Returns(Task.FromResult<IEnumerable<IncidentListModel>>(expected));

            var service = new SecureIncidentService(
                _MockIncidentRepository.Object,
                null, null, null, null, null, null, null, new AuthorizationMock().Object, null, null);

            // act
            var result = (await service.GetIncidentListModelsAsync()).ToList();
            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetIncidentListModelsAsyncReturnsEmptyList()
        {
            // arrange
            var _MockIncidentRepository = new Mock<IIncidentRepository>();

            List<IncidentListModel> expected = new List<IncidentListModel>();

            _MockIncidentRepository
                .Setup(p => p.GetIncidentListModelsAsync())
                .Returns(Task.FromResult<IEnumerable<IncidentListModel>>(expected));

            var service = new SecureIncidentService(
                _MockIncidentRepository.Object,
                null, null, null, null, null, null, null, new AuthorizationMock().Object, null, null);

            // act
            var result = (await service.GetIncidentListModelsAsync()).ToList();
            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task UpdateTransportUnitReturnsTrue()
        {
            var mockTransportUnitRepository = new Mock<ITransportUnitRepository>();
            var model = new IncidentDetailsModel
            {
                Code = "1234",
                TransportUnit = new UnidadDeTransporte { Matricula = "ABC123" },
                TransportUnitId = "ABC123"
            };
            var incident = new Incidente
            {
                Codigo = "1234",
            };
            mockTransportUnitRepository
                .Setup(t => t.GetByKeyAsync(model.TransportUnitId))
                .Returns(Task.FromResult<UnidadDeTransporte>(model.TransportUnit));
            var service = new SecureIncidentService(null, null, null, null,
                    mockTransportUnitRepository.Object, null, null, null, new AuthorizationMock().Object, null, null);
            var result = await service.UpdateTransportUnit(model, incident);
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateTransportUnitSameUnitsReturnsFalse()
        {
            var mockTransportUnitRepository = new Mock<ITransportUnitRepository>();
            var model = new IncidentDetailsModel
            {
                Code = "1234",
                TransportUnit = new UnidadDeTransporte { Matricula = "ABC123" },
                TransportUnitId = "ABC123"
            };
            var incident = new Incidente
            {
                Codigo = "1234",
                MatriculaTrans = "ABC123"     //MatriculaTrans is the same, so an update isn't needed. Returns false
            };
            mockTransportUnitRepository
                .Setup(t => t.GetByKeyAsync(model.TransportUnitId))
                .Returns(Task.FromResult<UnidadDeTransporte>(model.TransportUnit));
            var service = new SecureIncidentService(null, null, null, null,
                    mockTransportUnitRepository.Object, null, null, null, new AuthorizationMock().Object, null, null);
            var result = await service.UpdateTransportUnit(model, incident);
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateTransportUnitNullUnitReturnsFalse()
        {
            var mockTransportUnitRepository = new Mock<ITransportUnitRepository>();
            var model = new IncidentDetailsModel
            {
                Code = "1234",      // TransportUnit is null.
            };
            var incident = new Incidente
            {
                Codigo = "1234",
                MatriculaTrans = "ABC123"
            };
            var service = new SecureIncidentService(null, null, null, null,
                    mockTransportUnitRepository.Object, null, null, null, new AuthorizationMock().Object, null, null);
            var result = await service.UpdateTransportUnit(model, incident);
            Assert.False(result);
        }

        [Fact]
        public async Task GetNextIncidentStateReturnsApproved()
        {
            // arrange
            var mockIncidentRepository = new Mock<IIncidentRepository>();
            var mockStatesRepository = new Mock<IIncidentStateRepository>();
            var currentState = new Estado { Nombre = "Creado" };
            var nextState = new Estado { Nombre = "Aprobado" };

            mockStatesRepository
                .Setup(p => p.GetCurrentStateByIncidentId(String.Empty))
                .Returns(Task.FromResult<Estado>(currentState));

            var service = new SecureIncidentService(
                mockIncidentRepository.Object,
                null, mockStatesRepository.Object, null, null, null, null, null, null, null, null);

            // act
            var result = await service.GetNextIncidentState(String.Empty);

            // assert
            Assert.Equal(nextState.Nombre, result);

        }

        [Fact]
        public async Task GetNextIncidentStateReturnsEmptyString()
        {
            // arrange
            var mockIncidentRepository = new Mock<IIncidentRepository>();
            var mockStatesRepository = new Mock<IIncidentStateRepository>();
            var currentState = new Estado { Nombre = "Finalizado" };
            var nextState = new Estado { Nombre = "" };

            mockStatesRepository
                .Setup(p => p.GetCurrentStateByIncidentId(String.Empty))
                .Returns(Task.FromResult<Estado>(currentState));

            var service = new SecureIncidentService(
                mockIncidentRepository.Object,
                null, mockStatesRepository.Object, null, null, null, null, null, null, null, null);

            // act
            var result = await service.GetNextIncidentState(String.Empty);

            // assert
            Assert.Equal(nextState.Nombre, result);

        }

        [Fact]
        public async Task GetPendingTaskAsyncForCreatedState()
        {
            // arrange
            var mockIncidentRepository = new Mock<IIncidentRepository>();
            string nextState = "Creado";
            var model = new IncidentDetailsModel
            {
                Code = "1234",
            };
            List<Tuple<string, string>> expected = new List<Tuple<string, string>> {
                Tuple.Create("Seleccionar origen", "Origin"),
                Tuple.Create("Seleccionar destino", "Destination"),
                Tuple.Create("Agregar paciente", "Patient")
            };


            var service = new SecureIncidentService(
                mockIncidentRepository.Object,
                null, null, null, null, null, null, null, null, null, null);

            // act
            var result = await service.GetPendingTasksAsync(model, nextState);

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetPendingTaskAsyncForApprovedState()
        {
            // arrange
            var mockIncidentRepository = new Mock<IIncidentRepository>();
            string nextState = "Aprobado";
            var model = new IncidentDetailsModel
            {
                Code = "1234",
                CurrentState = "Creado"
            };
            List<Tuple<string, string>> expected = new List<Tuple<string, string>> {
                Tuple.Create("Esperando revisión", "Info"),
            };


            var service = new SecureIncidentService(
                mockIncidentRepository.Object,
                null, null, null, null, null, null, null, null, null, null);

            // act
            var result = await service.GetPendingTasksAsync(model, nextState);

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetPendingTaskAsyncForRejectedState()
        {
            // arrange
            var mockIncidentRepository = new Mock<IIncidentRepository>();
            string nextState = "Aprobado";
            var model = new IncidentDetailsModel
            {
                Code = "1234",
            };
            List<Tuple<string, string>> expected = new List<Tuple<string, string>> {
                Tuple.Create("Esperando una nueva revisión", "Info")
            };


            var service = new SecureIncidentService(
                mockIncidentRepository.Object,
                null, null, null, null, null, null, null, null, null, null);

            // act
            var result = await service.GetPendingTasksAsync(model, nextState);

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetPendingTaskAsyncForAssignedState()
        {
            // arrange
            var mockIncidentRepository = new Mock<IIncidentRepository>();
            var mockAssignmentRepository = new Mock<IAssignmentRepository>();
            IEnumerable<EspecialistaTécnicoMédico> teamMembers = new List<EspecialistaTécnicoMédico>();
            string nextState = "Asignado";
            var model = new IncidentDetailsModel
            {
                Code = "1234",
            };
            var incident = new Incidente
            {
                Codigo = "1234"
            };

            mockIncidentRepository
             .Setup(p => p.GetByKeyAsync(model.Code))
             .Returns(Task.FromResult<Incidente>(incident));

            mockAssignmentRepository
              .Setup(p => p.GetAssignmentsByIncidentIdAsync(incident.Codigo))
              .Returns(Task.FromResult<IEnumerable<EspecialistaTécnicoMédico>>(teamMembers));


            List<Tuple<string, string>> expected = new List<Tuple<string, string>> {
                Tuple.Create("Seleccionar unidad de transporte", "Assignment"),
                Tuple.Create("Seleccionar coordinador", "Assignment"),
                Tuple.Create("Seleccionar técnicos médicos", "Assignment")
            };


            var service = new SecureIncidentService(
                mockIncidentRepository.Object,
                null, null, null, null, null, null, mockAssignmentRepository.Object, null, null, null);

            // act
            var result = await service.GetPendingTasksAsync(model, nextState);

            // assert
            Assert.Equal(expected, result);
        }
    }
}
