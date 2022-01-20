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
    public class UsuarioIntegrationTests : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        public UsuarioIntegrationTests(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllUsersWithDetailsAsyncReturnsNotNull()
        {
            var usuarioService = _factory.Services.GetRequiredService<IUserService>();
            var result = await usuarioService.GetAllUsersWithDetailsAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetUsuarioWithDetailsReturnsValidUser()
        {
            var usuarioService = _factory.Services.GetRequiredService<IUserService>();
            var result = await usuarioService.getUsuarioWithDetailsAsync("a6f7aa70-a038-419f-9945-7c77b093d58f");
            Assert.Equal("a6f7aa70-a038-419f-9945-7c77b093d58f", result.Id);
            Assert.Equal("juan.guzman@prime.com", result.Email);
            Assert.Equal("AQAAAAEAACcQAAAAEKBfjZVSMkEvJ3kJikd/FETuy1hxI3csK3qM2EwHBlQpgixfBX3tUaxpposHbUfakg==", result.PasswordHash);
            Assert.Equal("M7SUOG4MXMPBKLX2BN34HVOG7GRGNIDQ", result.SecurityStamp);
        }

        [Fact]
        public async Task GetPersonWithDetailstAsyncReturnsValue ()
        {
            var usuarioService = _factory.Services.GetRequiredService<IUserService>();
            var result = await usuarioService.getPersonWithDetailstAsync("juan.guzman@prime.com");
            var ced = result.Cédula;
            Assert.Equal("12345678", ced);
        }

        [Fact]
        public async Task GetPersonWithDetailstAsyncReturnsNull()
        {
            var usuarioService = _factory.Services.GetRequiredService<IUserService>();
            var result = await usuarioService.getPersonWithDetailstAsync("invalid value");
            Assert.Null(result);
        }

        [Fact]
        public async Task GetNotAuthenticatedUsersReturnsNotNull()
        {
            var usuarioService = _factory.Services.GetRequiredService<IUserService>();
            var result = await usuarioService.GetNotAuthenticatedUsers();
            var value = result.Find(c => c.CedPersona == "11111111");
            Assert.NotNull(value);
        }

        [Fact]
        public async Task GetUserFormFromRegisterUserFormAsyncReturnNullTest()
        {
            var usuarioService = _factory.Services.GetRequiredService<IUserService>();
            RegisterUserFormModel registerUserForm = null;
            var result = await usuarioService.GetUserFormFromRegisterUserFormAsync(registerUserForm);
            Assert.Null(result);
        }


        [Fact]
        public async Task GetUserFormFromRegisterUserFormAsyncReturnValidUserFormTest()
        {
            var usuarioService = _factory.Services.GetRequiredService<IUserService>();

            RegisterUserFormModel registerUserForm = new RegisterUserFormModel
            {
                IdCardNumber = "12345678",
                Name = "Juan",
                FirstLastName = "Guzman",
                Email = "juan.guzman@prime.com"
            };

            var result = await usuarioService.GetUserFormFromRegisterUserFormAsync(registerUserForm);
            Assert.Equal("12345678", result.IdCardNumber);
            Assert.Equal("juan.guzman@prime.com", result.Email);
        }

        [Fact]
        public async Task StoreUserAsyncReturnsFalse()
        {
            var usuarioService = _factory.Services.GetRequiredService<IUserService>();
            var userToRegist = new UserFormModel
            {
                Email = "juan.guzman@prime.com",
                IdCardNumber = "12345678",
            };
            var value = await usuarioService.StoreUserAsync(userToRegist);
            Assert.False(value);
        }

        [Fact]
        public async Task StoreUserAsyncReturnsTrue()
        {
            var personaService = _factory.Services.GetRequiredService<IPersonService>();

            var personToRegist = new PersonFormModel
            {
                IdCardNumber = "117980341",
                Name = "Luis",
                FirstLastName = "Sanchez",
                SecondLastName = "Romero"
            };
            await personaService.StoreNewPersonAsync(personToRegist);

            var usuarioService = _factory.Services.GetRequiredService<IUserService>();
            var userToRegist = new UserFormModel
            {
                Email = "luisandres2712@gmail.com",
                IdCardNumber = "117980341",
            };
            var value = await usuarioService.StoreUserAsync(userToRegist);
            Assert.True(value);
        }

    }
}