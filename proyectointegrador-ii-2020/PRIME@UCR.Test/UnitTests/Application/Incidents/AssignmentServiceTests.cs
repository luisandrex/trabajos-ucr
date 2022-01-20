using Moq;
using PRIME_UCR.Application.Implementations.Incidents;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models.Incidents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using NuGet.Frameworks;
using PRIME_UCR.Domain.Models;
using Castle.DynamicProxy.Generators;
using Microsoft.AspNetCore.DataProtection;
using PRIME_UCR.Application.Permissions.Incidents;

namespace PRIME_UCR.Test.UnitTests.Application.Incidents
{
    public class AssignmentServiceTest
    {
        [Fact]
        public async Task GetAllTransportUnitsByModeReturnsEmpty()
        {
            var mockRepo = new Mock<ITransportUnitRepository>();
            mockRepo.Setup(p => p.GetAllTransporUnitsByMode(String.Empty))
                .Returns(Task.FromResult<IEnumerable<UnidadDeTransporte>>(new List<UnidadDeTransporte>()));
            var assignmentService = new SecureAssignmentService(mockRepo.Object, null, null, null, null, new AuthorizationMock().Object);
            var result = await assignmentService.GetAllTransportUnitsByMode(String.Empty);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllTransportUnitsByModeReturnsQuantity()
        {
            /*If there are coordinators registered, the service should not return an empty array.
             */
            var mockRepo = new Mock<ITransportUnitRepository>();
            List<UnidadDeTransporte> MyList = new List<UnidadDeTransporte>
            {
                new UnidadDeTransporte(),
                new UnidadDeTransporte(),
                new UnidadDeTransporte(),
                new UnidadDeTransporte(),
            };
            mockRepo
                .Setup(p =>
                    p.GetAllTransporUnitsByMode("Accion"))
                        .Returns(Task.FromResult<IEnumerable<UnidadDeTransporte>>(MyList)
                );
            var assignmentService = new SecureAssignmentService(mockRepo.Object, null, null, null, null, new AuthorizationMock().Object);
            var result = await assignmentService.GetAllTransportUnitsByMode("Accion");
            Assert.NotEmpty(result);
            Assert.Equal(MyList, result.ToList());

        }

        [Fact]
        public async Task GetAssignmentsByIncidentIdAsyncReturnsValid()
        {
            var mockRepo = new Mock<IIncidentRepository>();
            var mockRepo1 = new Mock<ICoordinadorTécnicoMédicoRepository>();
            var mockRepo2 = new Mock<ITransportUnitRepository>();
            var mockRepo3 = new Mock<IAssignmentRepository>();
            var incident = new Incidente
            {
                Codigo = "12",
                CedulaTecnicoCoordinador = "11111111",
                MatriculaTrans = "XYX"
            };
            var coordinator = new CoordinadorTécnicoMédico {};
            var TUnit = new UnidadDeTransporte
            {
                Matricula = "XYX"
            };
            var specialist = new List<EspecialistaTécnicoMédico>
            {
                new EspecialistaTécnicoMédico()
            };
            mockRepo.Setup(p => p.GetByKeyAsync(incident.Codigo))
                .Returns(Task.FromResult<Incidente>(incident));
            mockRepo1.Setup(p => p.GetByKeyAsync(incident.CedulaTecnicoCoordinador))
                .Returns(Task.FromResult<CoordinadorTécnicoMédico>(coordinator));
            mockRepo2.Setup(p => p.GetByKeyAsync(incident.MatriculaTrans))
                .Returns(Task.FromResult<UnidadDeTransporte>(TUnit));
            mockRepo3.Setup(p => p.GetAssignmentsByIncidentIdAsync(incident.Codigo))
                .Returns(Task.FromResult<IEnumerable<EspecialistaTécnicoMédico>>(specialist));
            var assignmentService = new SecureAssignmentService(mockRepo2.Object, mockRepo1.Object, null, mockRepo3.Object, mockRepo.Object, new AuthorizationMock().Object);
            var result = await assignmentService.GetAssignmentsByIncidentIdAsync(incident.Codigo);
            Assert.Equal(incident.MatriculaTrans, result.TransportUnit.Matricula);
        }

        [Fact]
        public async Task GetAssignmentsByIncidentIdAsyncReturnsNull()
        {
            var mockRepo = new Mock<IIncidentRepository>();
            mockRepo.Setup(p => p.GetByKeyAsync(String.Empty))
                .Returns(Task.FromResult<Incidente>(null));
            var assignmentService = new SecureAssignmentService(null, null, null, null, mockRepo.Object, new AuthorizationMock().Object);
            await Assert.ThrowsAsync<ArgumentException>(() => assignmentService.GetAssignmentsByIncidentIdAsync(String.Empty));
        }

        [Fact]
        public async Task GetCoordinatorsAsyncReturnsEmpty()
        {
            /*If there are no coordinators registered, the service should return an empty array.
             */
            var mockRepo = new Mock<ICoordinadorTécnicoMédicoRepository>();
            mockRepo
                .Setup(p =>
                    p.GetAllAsync()).
                        Returns(Task.FromResult<IEnumerable<CoordinadorTécnicoMédico>>(new List<CoordinadorTécnicoMédico>())
                );
            var AssignmentService = new SecureAssignmentService(null, mockRepo.Object, null, null, null, new AuthorizationMock().Object);
            var result = await AssignmentService.GetCoordinatorsAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCoordinatorsAsyncReturnsQuantity()
        {
            /*If there are coordinators registered, the service should not return an empty array.
             */
            var mockRepo = new Mock<ICoordinadorTécnicoMédicoRepository>();
            List<CoordinadorTécnicoMédico> MyList = new List<CoordinadorTécnicoMédico>
            {
                new CoordinadorTécnicoMédico(),
                new CoordinadorTécnicoMédico(),
                new CoordinadorTécnicoMédico(),
                new CoordinadorTécnicoMédico(),
            };
            mockRepo
                .Setup(p =>
                    p.GetAllAsync())
                        .Returns(Task.FromResult<IEnumerable<CoordinadorTécnicoMédico>>(MyList)
                );
            var AssignmentService = new SecureAssignmentService(null, mockRepo.Object, null, null, null, new AuthorizationMock().Object);
            var result = await AssignmentService.GetCoordinatorsAsync();
            Assert.NotEmpty(result);
            Assert.Equal(MyList, result.ToList());
        }

        [Fact]
        public async Task AssignToIncidentAsyncRuns()
        {
            /*If the service receives valid entries it should run flawlessly.
             */
            var IncidentRepo = new Mock<IIncidentRepository>();
            var AssignmentRepo = new Mock<IAssignmentRepository>();
            string ParameterCode = "TestValue";
            Incidente IncidentToReturn = new Incidente();

            IncidentRepo
                .Setup(p =>
                    p.GetByKeyAsync(ParameterCode))
                        .Returns(Task.FromResult<Incidente>(IncidentToReturn));

            UnidadDeTransporte TransportUnitToTest = new UnidadDeTransporte
            {
                Matricula = "1234567"
            };
            CoordinadorTécnicoMédico AssignedCoordinatorToTest = new CoordinadorTécnicoMédico
            {
                Cédula = "1234567"
            };
            AssignmentModel ParameterModel = new AssignmentModel
            {
                Coordinator = AssignedCoordinatorToTest,
                TransportUnit = TransportUnitToTest,
                TeamMembers = new List<EspecialistaTécnicoMédico>
                {
                    new EspecialistaTécnicoMédico{ Cédula = "123456789" }
                }
            };
            var AssignmentService = new SecureAssignmentService(null, null, null, AssignmentRepo.Object, IncidentRepo.Object, new AuthorizationMock().Object);
            await AssignmentService.AssignToIncidentAsync(ParameterCode, ParameterModel);
        }

        [Fact]
        public async Task GetSpecialistsAsyncReturnsEmpty()
        {
            /*There are no Specialists test case -> returns empty list*/
            var mockRepo = new Mock<IEspecialistaTécnicoMédicoRepository>();
            mockRepo
                .Setup(p =>
                    p.GetAllAsync()).
                        Returns(Task.FromResult<IEnumerable<EspecialistaTécnicoMédico>>(new List<EspecialistaTécnicoMédico>())
                );
            var AssignmentService = new SecureAssignmentService(null, null, mockRepo.Object, null, null, new AuthorizationMock().Object);
            var result = await AssignmentService.GetSpecialistsAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSpecialistsAsyncReturnsQuantity()
        {
            /* Positive test case, will check the amount of Specialists returned,
             * with the amount of Specialists that exists.
            */
            var mockRepo = new Mock<IEspecialistaTécnicoMédicoRepository>();
            List<EspecialistaTécnicoMédico> MyList = new List<EspecialistaTécnicoMédico>
            {
                new EspecialistaTécnicoMédico(),
                new EspecialistaTécnicoMédico(),
                new EspecialistaTécnicoMédico(),
            };
            mockRepo
                .Setup(p =>
                    p.GetAllAsync())
                        .Returns(Task.FromResult<IEnumerable<EspecialistaTécnicoMédico>>(MyList)
                );
            var AssignmentService = new SecureAssignmentService(null, null, mockRepo.Object, null, null, new AuthorizationMock().Object);
            var result = await AssignmentService.GetSpecialistsAsync();
            Assert.NotEmpty(result);
            Assert.Equal(MyList, result.ToList());
        }

        [Fact]
        public async Task IsAuthorizedToViewPatientWithCoordinatorReturnsTrue()
        {
            /* Case: There is a coordinator linked with incident. The coordinator is
             * the one who is checking the patient info.
             * -> Returns true because the coordinator is authorized
            */
            var IncidentRepo = new Mock<IIncidentRepository>();
            var CoordinatorRepo = new Mock<ICoordinadorTécnicoMédicoRepository>();
            var TransportRepo = new Mock<ITransportUnitRepository>();
            var AssignmentRepo = new Mock<IAssignmentRepository>();
            //Coordinator
            var coordinator = new CoordinadorTécnicoMédico { Cédula = "987654321" };
            var incident = new Incidente
            {
                Codigo = "0101",
                MatriculaTrans = "LOL",
                CedulaTecnicoCoordinador = coordinator.Cédula,
            };
            var TUnit = new UnidadDeTransporte
            {
                Matricula = "LOL"
            };
            var specialist = new List<EspecialistaTécnicoMédico>
            {
                new EspecialistaTécnicoMédico()
            };
            // Repositories setup
            IncidentRepo.Setup(p => p.GetByKeyAsync(incident.Codigo))
                .Returns(Task.FromResult<Incidente>(incident));
            IncidentRepo.Setup(p => p.GetAssignedOriginDoctor(String.Empty))
                .Returns(Task.FromResult<Médico>(null));
            IncidentRepo.Setup(p => p.GetAssignedDestinationDoctor(String.Empty))
                .Returns(Task.FromResult<Médico>(null));
            CoordinatorRepo.Setup(p => p.GetByKeyAsync(incident.CedulaTecnicoCoordinador))
                .Returns(Task.FromResult<CoordinadorTécnicoMédico>(coordinator));
            TransportRepo.Setup(p => p.GetByKeyAsync(incident.MatriculaTrans))
                .Returns(Task.FromResult<UnidadDeTransporte>(TUnit));
            AssignmentRepo.Setup(p => p.GetAssignmentsByIncidentIdAsync(incident.Codigo))
                .Returns(Task.FromResult<IEnumerable<EspecialistaTécnicoMédico>>(specialist));

            // call to service
            var AssignmentService = new SecureAssignmentService(TransportRepo.Object, CoordinatorRepo.Object, null, AssignmentRepo.Object, IncidentRepo.Object, new AuthorizationMock().Object);
            var result = await AssignmentService.IsAuthorizedToViewPatient(incident.Codigo, incident.CedulaTecnicoCoordinador);
            Assert.True(result);
        }

        [Fact]
        public async Task IsAuthorizedToViewPatientWithSpecialistReturnsTrue()
        {
            /* Case: There is a specialist linked with incident. The specialist is
             * the one who is checking the patient info.
             * -> Returns true because the specialist is authorized
            */
            var IncidentRepo = new Mock<IIncidentRepository>();
            var CoordinatorRepo = new Mock<ICoordinadorTécnicoMédicoRepository>();
            var TransportRepo = new Mock<ITransportUnitRepository>();
            var AssignmentRepo = new Mock<IAssignmentRepository>();
            //Specialist
            var specialist = new EspecialistaTécnicoMédico { Cédula = "987654321" };
            var incident = new Incidente
            {
                Codigo = "0101",
                MatriculaTrans = "LOL",
            };
            var TUnit = new UnidadDeTransporte
            {
                Matricula = "LOL"
            };
            var specialists = new List<EspecialistaTécnicoMédico>
            {
                specialist
            };
            // Repositories setup
            IncidentRepo.Setup(p => p.GetByKeyAsync(incident.Codigo))
                .Returns(Task.FromResult<Incidente>(incident));
            IncidentRepo.Setup(p => p.GetAssignedOriginDoctor(String.Empty))
                .Returns(Task.FromResult<Médico>(null));
            IncidentRepo.Setup(p => p.GetAssignedDestinationDoctor(String.Empty))
                .Returns(Task.FromResult<Médico>(null));
            CoordinatorRepo.Setup(p => p.GetByKeyAsync(String.Empty))
                .Returns(Task.FromResult<CoordinadorTécnicoMédico>(null));
            TransportRepo.Setup(p => p.GetByKeyAsync(incident.MatriculaTrans))
                .Returns(Task.FromResult<UnidadDeTransporte>(TUnit));
            AssignmentRepo.Setup(p => p.GetAssignmentsByIncidentIdAsync(incident.Codigo))
                .Returns(Task.FromResult<IEnumerable<EspecialistaTécnicoMédico>>(specialists));

            // call to service
            var AssignmentService = new SecureAssignmentService(TransportRepo.Object, CoordinatorRepo.Object, null, AssignmentRepo.Object, IncidentRepo.Object, new AuthorizationMock().Object);
            var result = await AssignmentService.IsAuthorizedToViewPatient(incident.Codigo, specialist.Cédula);
            Assert.True(result);
        }

        [Fact]
        public async Task IsAuthorizedToViewPatientReturnsFalse()
        {
            /* Case: There isn't any person authorized related with the incident.
             * -> Returns false, because the person requesting to view, is not authorized.
            */
            var IncidentRepo = new Mock<IIncidentRepository>();
            var CoordinatorRepo = new Mock<ICoordinadorTécnicoMédicoRepository>();
            var TransportRepo = new Mock<ITransportUnitRepository>();
            var AssignmentRepo = new Mock<IAssignmentRepository>();
            var incident = new Incidente
            {
                Codigo = "0101",
                MatriculaTrans = "CQL",
            };
            var TUnit = new UnidadDeTransporte
            {
                Matricula = "CQL"
            };
            var specialists = new List<EspecialistaTécnicoMédico>
            {
                 new EspecialistaTécnicoMédico()
            };
            // Repositories setup
            IncidentRepo.Setup(p => p.GetByKeyAsync(incident.Codigo))
                .Returns(Task.FromResult<Incidente>(incident));
            IncidentRepo.Setup(p => p.GetAssignedOriginDoctor(String.Empty))
                .Returns(Task.FromResult<Médico>(null));
            IncidentRepo.Setup(p => p.GetAssignedDestinationDoctor(String.Empty))
                .Returns(Task.FromResult<Médico>(null));
            CoordinatorRepo.Setup(p => p.GetByKeyAsync(String.Empty))
                .Returns(Task.FromResult<CoordinadorTécnicoMédico>(null));
            TransportRepo.Setup(p => p.GetByKeyAsync(incident.MatriculaTrans))
                .Returns(Task.FromResult<UnidadDeTransporte>(TUnit));
            AssignmentRepo.Setup(p => p.GetAssignmentsByIncidentIdAsync(incident.Codigo))
                .Returns(Task.FromResult<IEnumerable<EspecialistaTécnicoMédico>>(specialists));

            // call to service
            var AssignmentService = new SecureAssignmentService(TransportRepo.Object, CoordinatorRepo.Object, null, AssignmentRepo.Object, IncidentRepo.Object, new AuthorizationMock().Object);
            var result = await AssignmentService.IsAuthorizedToViewPatient(incident.Codigo, "Cedula invalida");
            Assert.False(result);
        }

        [Fact]
        public async Task IsAuthorizedToViewPatientWithDoctorsReturnsTrue()
        {
            /*Case: There is an origin doctor.  */
            var IncidentRepo = new Mock<IIncidentRepository>();
            var CoordinatorRepo = new Mock<ICoordinadorTécnicoMédicoRepository>();
            var TransportRepo = new Mock<ITransportUnitRepository>();
            var AssignmentRepo = new Mock<IAssignmentRepository>();
            //Origin doctor
            var originDoctor = new Médico { Cédula = "123456789" };
            var destinationDoctor = new Médico { Cédula = "987654321" };
            var coordinator = new CoordinadorTécnicoMédico { Cédula = "987654321" };
            var incident = new Incidente
            {
                Codigo = "0101",
                MatriculaTrans = "LOL",
                CedulaTecnicoCoordinador = coordinator.Cédula,
                Origen = new CentroUbicacion { CedulaMedico = originDoctor.Cédula, Médico = originDoctor },
                Destino = new CentroUbicacion { CedulaMedico = destinationDoctor.Cédula, Médico = destinationDoctor }
            };
            var TUnit = new UnidadDeTransporte
            {
                Matricula = "LOL"
            };
            var specialist = new List<EspecialistaTécnicoMédico>
            {
                new EspecialistaTécnicoMédico()
            };
            IncidentRepo.Setup(p => p.GetByKeyAsync(incident.Codigo))
                .Returns(Task.FromResult(incident));
            IncidentRepo.Setup(p => p.GetAssignedOriginDoctor(incident.Codigo))
                .Returns(Task.FromResult(originDoctor));
            IncidentRepo.Setup(p => p.GetAssignedDestinationDoctor(incident.Codigo))
                .Returns(Task.FromResult(destinationDoctor));
            CoordinatorRepo.Setup(p => p.GetByKeyAsync(incident.CedulaTecnicoCoordinador))
                .Returns(Task.FromResult(coordinator));
            TransportRepo.Setup(p => p.GetByKeyAsync(incident.MatriculaTrans))
                .Returns(Task.FromResult(TUnit));
            AssignmentRepo.Setup(p => p.GetAssignmentsByIncidentIdAsync(incident.Codigo))
                .Returns(Task.FromResult<IEnumerable<EspecialistaTécnicoMédico>>(specialist));

            // call to service
            var AssignmentService = new SecureAssignmentService(TransportRepo.Object, CoordinatorRepo.Object, null, AssignmentRepo.Object, IncidentRepo.Object, new AuthorizationMock().Object);
            var result = await AssignmentService.IsAuthorizedToViewPatient(incident.Codigo, destinationDoctor.Cédula);
            Assert.True(result);
        }

        [Fact]
        public async Task GetAssignedDestinationDoctorReturnsValid()
        {
            // arrange
            var _MockIncidentRepository = new Mock<IIncidentRepository>();
            var incident1 = new IncidentListModel { Codigo = "123abc" };
            var incident2 = new IncidentListModel { Codigo = "456def" };
            var incident3 = new IncidentListModel { Codigo = "789ghi" };
            string code = "código válido";
            Médico expected = new Médico { Cédula = "122873402" };

            _MockIncidentRepository
                .Setup(p => p.GetAssignedDestinationDoctor(code))
                .Returns(Task.FromResult(expected));


            var AssignmentService = new SecureAssignmentService(null, null, null, null, _MockIncidentRepository.Object, new AuthorizationMock().Object);


            // act
            var result = (await AssignmentService.GetAssignedDestinationDoctor(code));
            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAssignedDestinationDoctorReturnsNull()
        {
            // arrange
            var _MockIncidentRepository = new Mock<IIncidentRepository>();
            var incident1 = new IncidentListModel { Codigo = "123abc" };
            var incident2 = new IncidentListModel { Codigo = "456def" };
            var incident3 = new IncidentListModel { Codigo = "789ghi" };
            string code = "código válido";
            Médico expected = null;

            _MockIncidentRepository
                .Setup(p => p.GetAssignedDestinationDoctor(code))
                .Returns(Task.FromResult(expected));


            var assignmentService = new SecureAssignmentService(null, null, null, null, _MockIncidentRepository.Object, new AuthorizationMock().Object);


            // act
            var result = (await assignmentService.GetAssignedDestinationDoctor(code));
            // assert
            Assert.Equal(expected, result);
        }
    }
}
