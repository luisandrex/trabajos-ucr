using Moq;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Permissions.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.UnitTests.Application.UserAdministration
{
    public class PersonServiceTest
    {
        [Fact]
        public async Task GetPersonByIdAsyncReturnsNullTest()
        {
            
            var mockRepo = new Mock<IPersonaRepository>();
            mockRepo.Setup(p => p.GetByKeyPersonaAsync(String.Empty)).Returns(Task.FromResult<Persona>(null));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var personService = new SecurePersonService(mockRepo.Object, mockSecurity.Object);

            var result = await personService.GetPersonByIdAsync(String.Empty);
            Assert.Null(result);

        }

        [Fact]
        public async Task GetPersonByIdAsyncReturnsValidPersonTest()
        {
            string id = "12345678";
            var mockRepo = new Mock<IPersonaRepository>();
            mockRepo.Setup(p => p.GetByKeyPersonaAsync(id)).Returns(Task.FromResult<Persona>(new Persona
            {
                Cédula = "12345678",
                Nombre = "Juan",
                PrimerApellido = "Guzman"
            }));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var personService = new SecurePersonService(mockRepo.Object, mockSecurity.Object);

            var result = await personService.GetPersonByIdAsync(id);
            
             Assert.Equal("12345678" , result.Cédula);
             Assert.Equal("Juan" , result.Nombre);
             Assert.Equal("Guzman" , result.PrimerApellido);
        }


        [Fact]
        public async Task DeletePersonAsyncTest()
        {
            string id = "12345678";

            var mockRepo = new Mock<IPersonaRepository>();
            mockRepo.Setup(p => p.DeleteAsync(id));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var personService = new SecurePersonService(mockRepo.Object, mockSecurity.Object);

            await personService.DeletePersonAsync(id);

        }

        

        [Fact]
        public async Task IStoreNewPersonAsyncTest()
        {
            Persona person = new Persona
            {
                Cédula = "17600410",
                Nombre = "Daniela",
                PrimerApellido = "Vargas"
            };

            var mockRepo = new Mock<IPersonaRepository>();
            mockRepo.Setup(p => p.InsertAsync(person));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var personService = new SecurePersonService(mockRepo.Object, mockSecurity.Object);

            PersonFormModel personFormModel = new PersonFormModel
            {
                IdCardNumber = person.Cédula,
                Name = person.Nombre, 
                FirstLastName = person.PrimerApellido
            };

            await personService.StoreNewPersonAsync(personFormModel);
        }

        [Fact]
        public async Task GetPersonModelFromRegisterModelAsyncReturnsValidPersonFormModelTest()
        {
            var mockRepo = new Mock<IPersonaRepository>();

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var personService = new SecurePersonService(mockRepo.Object, mockSecurity.Object);

            RegisterUserFormModel registerUserForm = new RegisterUserFormModel
            {
                IdCardNumber = "12345678", 
                Name = "Juan", 
                FirstLastName = "Guzman"
            };

            var result = await personService.GetPersonModelFromRegisterModelAsync(registerUserForm);

            Assert.Equal("12345678", result.IdCardNumber);
            Assert.Equal("Juan", result.Name);
            Assert.Equal("Guzman", result.FirstLastName);
        }
        
        [Fact]
        public async Task GetPersonModelFromRegisterModelAsyncReturnsNullTest()
        {

            var mockRepo = new Mock<IPersonaRepository>();

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var personService = new SecurePersonService(mockRepo.Object, mockSecurity.Object);

            RegisterUserFormModel registerUserForm = null;

            var result = await personService.GetPersonModelFromRegisterModelAsync(registerUserForm);
            Assert.Null(result);

        }

        [Fact]
        public async Task GetPersonByCedAsyncReturnsNullTest()
        {
            var mockRepo = new Mock<IPersonaRepository>();
            mockRepo.Setup(p => p.GetByCedPersonaAsync(String.Empty)).Returns(Task.FromResult<Persona>(null));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var personService = new SecurePersonService(mockRepo.Object, mockSecurity.Object);
            var result = await personService.GetPersonByCedAsync(String.Empty);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPersonByCedAsyncReturnsValidPerson()
        {
            var ced = "12345678";
            var mockRepo = new Mock<IPersonaRepository>();
            mockRepo.Setup(p => p.GetByCedPersonaAsync(ced)).Returns(Task.FromResult<Persona>(new Persona
            {
                Cédula = "12345678",
                Nombre = "Juan",
                PrimerApellido = "Guzman"
            }));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var personService = new SecurePersonService(mockRepo.Object, mockSecurity.Object);
            var result = await personService.GetPersonByCedAsync(ced);
            Assert.Equal("12345678", result.Cédula);
            Assert.Equal("Juan", result.Nombre);
            Assert.Equal("Guzman", result.PrimerApellido);
        }

    }
}
