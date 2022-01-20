using Moq;
using PRIME_UCR.Application.Implementations.MedicalRecords;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.Permissions.MedicalRecord;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Constants;

namespace PRIME_UCR.Test.UnitTests.Application.MedicalRecords
{
    public class MedicalRecordServiceTest
    {
        [Fact]

        public async void getMedicalRecordByNotValidId()
        {

            var mockRepo = new Mock<IMedicalRecordRepository>();
            var mockRepoList = new Mock<IMedicalRecordRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            //Sets mocks for repositories
            mockRepo.Setup(p => p.GetDetailsRecordWithPatientDoctorDatesAsync(-1)).Returns(Task.FromResult<Expediente>(null));
            var MedRecordService = new SecureMedicalRecordService(mockRepo.Object, null, null, null, null, null, null, null, null, null, null, null,mockSecurity.Object);
            //Creates Service for test
            var result = await MedRecordService.GetMedicalRecordDetailsLinkedAsync(-1);
            //Asserts the result
            Assert.Null(result);

        }

        [Fact]
        public async void getMedicalRecordByValidIdRecord()
        {

            var mockRepo = new Mock<IMedicalRecordRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            //Sets mocks for repositories
            Expediente record = new Expediente()
            {
                CedulaPaciente = "123456789",
                CedulaMedicoDuenno = "222222222",
                FechaCreacion = DateTime.Now,
                Clinica = "Mexico",
                Paciente = null,
                Medico = null,
                Alergias = null,
                Antecedentes = null,
                PadecimientosCronicos = null,
                Citas = null
            };

            mockRepo.Setup(p => p.GetByPatientIdAsync("123456789")).Returns(Task.FromResult(record));
            var MedRecordService = new SecureMedicalRecordService(mockRepo.Object, null, null, null, null, null, null, null, null, null, null, null, mockSecurity.Object);
            //Creates Service for test
            var result = await MedRecordService.GetByPatientIdAsync("123456789");
            //Asserts the result
            Assert.Equal(record, result);
        }

        [Fact]

        public async void getMedicalRecordDetailedByValidIdentification()
        {

            var mockRepo = new Mock<IMedicalRecordRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            //Sets mocks for repositories
            Expediente record = new Expediente()
            {
                CedulaPaciente = "123456789",
                CedulaMedicoDuenno = "222222222",
                FechaCreacion = DateTime.Now,
                Clinica = "Mexico",
                Paciente = new Paciente() { Cédula = "123456789", Nombre = "Juan", PrimerApellido = "Perez" },
                Medico = new Médico() { Cédula = "222222222", Nombre = "Juan", PrimerApellido = "Guzman" },
                Alergias = null,
                Antecedentes = null,
                PadecimientosCronicos = null,
                Citas = null
            };

            mockRepo.Setup(p => p.GetDetailsRecordWithPatientDoctorDatesAsync(123456789)).Returns(Task.FromResult(record));
            var MedRecordService = new SecureMedicalRecordService(mockRepo.Object, null, null, null, null, null, null, null, null, null, null, null, mockSecurity.Object);
            //Creates Service for test
            var result = await MedRecordService.GetMedicalRecordDetailsLinkedAsync(123456789);
            //Asserts the result
            Assert.Equal(record, result);
        }


        [Fact]
        public async void updateMedicalRecord()
        {


            var mockRepo = new Mock<IMedicalRecordRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            //Sets mocks for repositories
            Expediente record = new Expediente()
            {
                CedulaPaciente = "123456789",
                CedulaMedicoDuenno = "222222222",
                FechaCreacion = DateTime.Now,
                Clinica = "Calderon",
                Paciente = new Paciente() { Cédula = "123456789", Nombre = "Juan", PrimerApellido = "Perez" },
                Medico = new Médico() { Cédula = "222222222", Nombre = "Juan", PrimerApellido = "Guzman" },
                Alergias = null,
                Antecedentes = null,
                PadecimientosCronicos = null,
                Citas = null
            };

            Expediente record_updated = new Expediente()
            {
                CedulaPaciente = "123456789",
                CedulaMedicoDuenno = "222222222",
                FechaCreacion = DateTime.Now,
                Clinica = "Calderon",
                Paciente = new Paciente() { Cédula = "123456789", Nombre = "Juan", PrimerApellido = "Perez" },
                Medico = new Médico() { Cédula = "222222222", Nombre = "Juan", PrimerApellido = "Guzman" },
                Alergias = null,
                Antecedentes = null,
                PadecimientosCronicos = null,
                Citas = null
            };

            mockRepo.Setup(p => p.UpdateMedicalRecordAsync(record)).Returns(Task.FromResult(record_updated));
            var MedRecordService = new SecureMedicalRecordService(mockRepo.Object, null, null, null, null, null, null, null, null, null, null, null, mockSecurity.Object);
            //Creates Service for test
            var result = await MedRecordService.UpdateMedicalRecordAsync(record);
            //Asserts the result
            Assert.Equal(record.CedulaMedicoDuenno, result.CedulaMedicoDuenno);
            Assert.Equal(record.CedulaPaciente, result.CedulaPaciente);
            Assert.Equal(record.Clinica, result.Clinica);
            Assert.Equal(record.Paciente.Nombre, result.Paciente.Nombre);
            Assert.Equal(record.Paciente.PrimerApellido, result.Paciente.PrimerApellido);
            Assert.Equal(record.Medico.Nombre, result.Medico.Nombre);
            Assert.Equal(record.Medico.PrimerApellido, result.Medico.PrimerApellido);
            Assert.Equal(record.Alergias, result.Alergias);
            Assert.Equal(record.Antecedentes, result.Antecedentes);
            Assert.Equal(record.PadecimientosCronicos, result.PadecimientosCronicos);
            Assert.Equal(record.Citas, result.Citas);
        }
    }
}
