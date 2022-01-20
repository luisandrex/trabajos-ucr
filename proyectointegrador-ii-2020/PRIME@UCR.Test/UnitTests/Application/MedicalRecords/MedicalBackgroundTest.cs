using System.Collections.Generic;
using Xunit;
using Moq;
using System.Threading.Tasks;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Application.Implementations.MedicalRecords;
using PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords;
using System;
using System.Linq;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Application.Permissions.MedicalRecord;

namespace PRIME_UCR.Test.UnitTests.Application.MedicalRecords
{
    public class MedicalBackgroundTest
    {
        [Fact]
        public async void getMedicalBackgroundByRecordIdNull()
        {
            var mockRepo = new Mock<IMedicalBackgroundRepository>();
            var mockRepoList = new Mock<IMedicalBackgroundListRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            mockRepo.Setup(p => p.GetByConditionAsync(i => i.IdExpediente == 0)).Returns(Task.FromResult<IEnumerable<Antecedentes>>(null));
            IMedicalBackgroundService MedicalBackgroundService
                            = new SecureMedicalBackgroundService(
                                           mockRepo.Object,
                                           mockRepoList.Object,
                                           mockSecurity.Object); 
            var result = await mockRepo.Object.GetByConditionAsync(a => a.IdExpediente == 0);
            var result2 = (await MedicalBackgroundService.GetBackgroundByRecordId(0));
            Assert.Null(result);
            Assert.Empty(result2);
        }

        [Fact]
        public async void getMedicalBackgroundByRecordIdInvalid()
        {
            var mockRepo = new Mock<IMedicalBackgroundRepository>();
            var mockRepoList = new Mock<IMedicalBackgroundListRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            //Sets mocks for repositories
            IMedicalBackgroundService MedicalBackgroundService
                            = new SecureMedicalBackgroundService(
                                           mockRepo.Object,
                                           mockRepoList.Object,
                                           mockSecurity.Object);
            //Creates Service for test
            var result = (await MedicalBackgroundService.GetBackgroundByRecordId(-1));
            //Asserts the result
            Assert.Empty(result);
        }

        [Fact]
        public async void GetAllAsyncNull()
        {
            var mockRepo = new Mock<IMedicalBackgroundRepository>();
            var mockRepoList = new Mock<IMedicalBackgroundListRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            //Sets mocks for repositories
            mockRepoList.Setup(p => p.GetAllAsync()).Returns(Task.FromResult<IEnumerable<ListaAntecedentes>>(null));
            //sets the return of the method to be tested
            IMedicalBackgroundService MedicalBackgroundService
                            = new SecureMedicalBackgroundService(
                                           mockRepo.Object, 
                                           mockRepoList.Object, 
                                           mockSecurity.Object);
            //Creates Service for test
            var result = await MedicalBackgroundService.GetAll();
            //Asserts the result
            Assert.Null(result);
        }

        [Fact]
        public async void GetAllAsyncNotNull()
        {
            var mockRepo = new Mock<IMedicalBackgroundRepository>();
            var mockRepoList = new Mock<IMedicalBackgroundListRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            //Sets mocks for repositories
            var pruebaLista = new ListaAntecedentes()
            {
                Id = 1,
                NombreAntecedente = "prueba"
            };
            List<ListaAntecedentes> listaAntecedentes = new List<ListaAntecedentes>
            {
                pruebaLista
            };
            mockRepoList.Setup(p => p.GetAllAsync()).Returns(Task.FromResult(listaAntecedentes.AsEnumerable()));
            IMedicalBackgroundService MedicalBackgroundService
                            = new SecureMedicalBackgroundService(
                                           mockRepo.Object,
                                           mockRepoList.Object,
                                           mockSecurity.Object);
            //Creates Service for test
            var result = await MedicalBackgroundService.GetAll();
            //Asserts the result
            Assert.Equal(pruebaLista.NombreAntecedente, result.FirstOrDefault().NombreAntecedente);
        }
    }
}

