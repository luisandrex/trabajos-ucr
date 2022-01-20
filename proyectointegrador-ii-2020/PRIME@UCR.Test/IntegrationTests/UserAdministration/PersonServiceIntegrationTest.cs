using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.UserAdministration
{
    public class PersonServiceIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        public PersonServiceIntegrationTest(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetPersonByIdAsyncReturnsNullTest()
        {
            var personService = _factory.Services.GetRequiredService<IPersonService>();
            var result = await personService.GetPersonByIdAsync(String.Empty);
            Assert.Null(result);

        }

        [Fact]
        public async Task GetPersonByIdAsyncReturnsValidPersonTest()
        {
            var personService = _factory.Services.GetRequiredService<IPersonService>();
            string id = "12345678";

            var result = await personService.GetPersonByIdAsync(id);

            Assert.Equal("12345678", result.Cédula);
            Assert.Equal("Juan", result.Nombre);
            Assert.Equal("Guzman", result.PrimerApellido);
        }

        [Fact]
        public async Task IStoreNewPersonAsyncTest()
        {
            PersonFormModel personFormModel = new PersonFormModel
            {
                IdCardNumber = "17600410",
                Name = "Daniela",
                FirstLastName = "Vargas"
            };
            var personService = _factory.Services.GetRequiredService<IPersonService>();

            await personService.StoreNewPersonAsync(personFormModel);
        }


        [Fact]
        public async Task DeletePersonAsyncTest()
        {
            string id = "17600410";
            var personService = _factory.Services.GetRequiredService<IPersonService>();

            await personService.DeletePersonAsync(id);

        }

        [Fact]
        public async Task GetPersonModelFromRegisterModelAsyncReturnsValidPersonFormModelTest()
        {
            var personService = _factory.Services.GetRequiredService<IPersonService>();

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
            var personService = _factory.Services.GetRequiredService<IPersonService>();

            RegisterUserFormModel registerUserForm = null;

            var result = await personService.GetPersonModelFromRegisterModelAsync(registerUserForm);
            Assert.Null(result);

        }

        [Fact]
        public async Task GetPersonByCedAsyncReturnsNullTest()
        {
            var personService = _factory.Services.GetRequiredService<IPersonService>();
            var result = await personService.GetPersonByCedAsync(String.Empty);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPersonByCedAsyncReturnsValidPerson()
        {
            var ced = "12345678";
            var personService = _factory.Services.GetRequiredService<IPersonService>();
            var result = await personService.GetPersonByCedAsync(ced);
            Assert.Equal("12345678", result.Cédula);
            Assert.Equal("Juan", result.Nombre);
            Assert.Equal("Guzman", result.PrimerApellido);
        }

    }
}
