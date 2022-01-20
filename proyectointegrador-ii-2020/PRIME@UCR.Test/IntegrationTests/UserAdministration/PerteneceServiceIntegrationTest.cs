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
    public class PerteneceServiceIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        public PerteneceServiceIntegrationTest(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task InsertUserOfProfileAsyncTest()
        {
            var perteneceService = _factory.Services.GetRequiredService<IPerteneceService>();
            string idUser = "12345678";
            string profile = "Médico";

            try
            {
                await perteneceService.InsertUserOfProfileAsync(idUser, profile);
            }
            catch (Exception)
            {
                var error = 1;
            }
            
        }

        [Fact]
        public async Task DeleteUserOfProfileAsyncTest()
        {
            var perteneceService = _factory.Services.GetRequiredService<IPerteneceService>();
            string idUser = "22222222";
            string profile = "Médico";

            try
            {
                await perteneceService.DeleteUserOfProfileAsync(idUser, profile);
            }
            catch (Exception)
            {
                var error = 1;
            }
        }
        /*
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

        }*/

    }
}