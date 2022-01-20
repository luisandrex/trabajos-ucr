using System.Collections.Generic;
using Xunit;
using Moq;
using System.Threading.Tasks;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Application.Implementations.MedicalRecords;
using PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords;
using PRIME_UCR.Application.Services.MedicalRecords;
using System;
using System.Linq;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.Permissions.MedicalRecord;
using PRIME_UCR.Domain.Constants;

namespace PRIME_UCR.Test.UnitTests.Application.MedicalRecords
{
    public class ChronicConditionServiceTest
    {
        [Fact]
        public async void getChronicConditionByRecordIdNull()
        {
            var mockRepo = new Mock<IChronicConditionRepository>();
            var mockRepoList = new Mock<IChronicConditionListRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            //Sets mocks for repositories
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            mockRepo.Setup(p => p.GetByConditionAsync(i => i.IdExpediente == 0)).Returns(Task.FromResult<IEnumerable<PadecimientosCronicos>>(null));
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            IChronicConditionService ChronicConditionService =
                new SecureChronicConditionService(mockRepo.Object,
                                                  mockRepoList.Object,
                                                  mockSecurity.Object);
            var result = await mockRepo.Object.GetByConditionAsync(a => a.IdExpediente == 0);
            var result2 = (await ChronicConditionService.GetChronicConditionByRecordId(0));
            Assert.Null(result);
            Assert.Empty(result2);
        }

        [Fact]
        public async void getChronicConditionByRecordIdInvalid()
        {
            var mockRepo = new Mock<IChronicConditionRepository>();
            var mockRepoList = new Mock<IChronicConditionListRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            //Sets mocks for repositories
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            IChronicConditionService ChronicConditionService =
                new SecureChronicConditionService(mockRepo.Object,
                                                  mockRepoList.Object,
                                                  mockSecurity.Object);
            //Creates Service for test
            var result = (await ChronicConditionService.GetChronicConditionByRecordId(-1));
            //Asserts the result
            Assert.Empty(result);
        }


        [Fact]
        public async void GetAllAsyncNull()
        {
            var mockRepo = new Mock<IChronicConditionRepository>();
            var mockRepoList = new Mock<IChronicConditionListRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            //Sets mocks for repositories
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            mockRepoList.Setup(p => p.GetAllAsync()).Returns(Task.FromResult<IEnumerable<ListaPadecimiento>>(null));
            IChronicConditionService ChronicConditionService =
                new SecureChronicConditionService(mockRepo.Object, 
                                                  mockRepoList.Object,  
                                                  mockSecurity.Object);
            //Creates Service for test
            var result = await ChronicConditionService.GetAll();
            //Asserts the result
            Assert.Null(result);
        }

        [Fact]
        public async void GetAllAsyncNotNull()
        {
            var mockRepo = new Mock<IChronicConditionRepository>();
            var mockRepoList = new Mock<IChronicConditionListRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            //Sets mocks for repositories
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            var pruebaLista = new ListaPadecimiento()
            {
                Id = 1,
                NombrePadecimiento = "prueba"
            };
            List<ListaPadecimiento> listaPadecimientos = new List<ListaPadecimiento>
            {
                pruebaLista
            };
            mockRepoList.Setup(p => p.GetAllAsync()).Returns(Task.FromResult(listaPadecimientos.AsEnumerable()));
            IChronicConditionService ChronicConditionService =
                new SecureChronicConditionService(mockRepo.Object,
                                                  mockRepoList.Object,
                                                  mockSecurity.Object);
            //Creates Service for test
            var result = await ChronicConditionService.GetAll();
            //Asserts the result
            Assert.Equal(pruebaLista.NombrePadecimiento, result.FirstOrDefault().NombrePadecimiento);
        }
    }
}

