using System.Collections.Generic;
using Xunit;
using Moq;
using System.Threading.Tasks;
using PRIME_UCR.Application.Repositories.MedicalRecords;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Application.Implementations.MedicalRecords;
using PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Application.Permissions.MedicalRecord;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Constants;

namespace PRIME_UCR.Test.UnitTests.Application.MedicalRecords
{
    public class AllergyServiceTest
    {

        [Fact]
        public async void getAllergyByRecordIdNull()
        {
            var mockRepo = new Mock<IAlergyRepository>();
            var mockRepoList = new Mock<IAlergyListRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            mockRepo.Setup(p => p.GetByConditionAsync(i => i.IdExpediente == 0)).Returns(Task.FromResult<IEnumerable<Alergias>>(null));
            IAlergyService AllergyService =
                    new SecureAlergyService(mockRepo.Object,
                                            mockRepoList.Object,
                                            mockSecurity.Object);
            var result = await mockRepo.Object.GetByConditionAsync(a => a.IdExpediente == 0);
            var result2 = (await AllergyService.GetAlergyByRecordId(0));
            Assert.Null(result);
            Assert.Empty(result2);
        }
        [Fact]
        public async void getAllergyByRecordIdInvalid()
        {
            var mockRepo = new Mock<IAlergyRepository>();
            var mockRepoList = new Mock<IAlergyListRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            //Sets mocks for repositories
            IAlergyService AllergyService =
                    new SecureAlergyService(mockRepo.Object,
                                            mockRepoList.Object,
                                            mockSecurity.Object);
            var result = (await AllergyService.GetAlergyByRecordId(-1));
            //Asserts the result
            Assert.Empty(result);
            //Assert.Null(result2);
        }

        [Fact]
        public async void GetAllAsyncNull()
        {
            var mockRepo = new Mock<IAlergyRepository>();
            var mockRepoList = new Mock<IAlergyListRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            //Sets mocks for repositories
            mockRepoList.Setup(p => p.GetAllAsync()).Returns(Task.FromResult<IEnumerable<ListaAlergia>>(null));
            IAlergyService AllergyService =
                    new SecureAlergyService(mockRepo.Object,
                                            mockRepoList.Object,
                                            mockSecurity.Object);
            //Creates Service for test
            var result = await AllergyService.GetAll();
            //Asserts the result
            Assert.Null(result);
        }

        [Fact]
        public async void GetAllAsyncNotNull()
        {
            var mockRepo = new Mock<IAlergyRepository>();
            var mockRepoList = new Mock<IAlergyListRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            //Sets mocks for repositories
            var pruebaLista = new ListaAlergia()
            {
                Id = 1,
                NombreAlergia = "prueba"
            };
            List<ListaAlergia> listaAlergias = new List<ListaAlergia>
            {
                pruebaLista
            };
            mockRepoList.Setup(p => p.GetAllAsync()).Returns(Task.FromResult(listaAlergias.AsEnumerable()));
            IAlergyService AllergyService =
                    new SecureAlergyService(mockRepo.Object,
                                            mockRepoList.Object,
                                            mockSecurity.Object);
            //Creates Service for test
            var result = await AllergyService.GetAll();
            //Asserts the result
            Assert.Equal(pruebaLista.NombreAlergia, result.FirstOrDefault().NombreAlergia);
        }
    }
}
