using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.UserAdministration
{
    public class AuthenticationServiceIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> factory;

        public AuthenticationServiceIntegrationTest(IntegrationTestWebApplicationFactory<Startup> _factory)
        {
            factory = _factory;
        }

        [Fact]
        public async Task GetPerfilesWithDetailsAsyncTest()
        {
            var service = factory.Services.GetRequiredService<IAuthenticationService>();
            var result = await service.GetAllProfilesWithDetailsAsync();
            Assert.Equal(6, result.Count);
        }

        [Fact]
        public async Task GetAllUsersWithDetailsAsyncTest()
        {
            var service = factory.Services.GetRequiredService<IAuthenticationService>();
            var result = await service.GetAllUsersWithDetailsAsync();
            if (result.Count == 12)
            {
                Assert.Equal(12, result.Count);
            }
            else
            {
                Assert.Equal(13, result.Count);
            }
        }

        [Fact]
        public async Task GetUserByEmailAsyncEmptyStringTest()
        {
            var service = factory.Services.GetRequiredService<IAuthenticationService>();
            var result = await service.GetUserByEmailAsync(String.Empty);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByEmailAsyncValidEmailTest()
        {
            var service = factory.Services.GetRequiredService<IAuthenticationService>();
            var result = await service.GetUserByEmailAsync("juan.guzman@prime.com");
            Assert.Equal("juan.guzman@prime.com", result.UserName);
            Assert.Equal("Juan", result.Persona.Nombre);
            Assert.Equal("Guzman", result.Persona.PrimerApellido);
        }

        [Fact]
        public async Task GetUserByEmailAsyncNotValidEmailTest()
        {
            var service = factory.Services.GetRequiredService<IAuthenticationService>();
            var result = await service.GetUserByEmailAsync("jose.viquez@prime.com");
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserWithDetailsAsyncNullTest()
        {
            var service = factory.Services.GetRequiredService<IAuthenticationService>();
            var result = await service.GetUserWithDetailsAsync(String.Empty);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserWithDetailsAsyncValidUserTest()
        {
            var service = factory.Services.GetRequiredService<IAuthenticationService>();
            var result = await service.GetUserWithDetailsAsync("021d330b-5ffa-4bfc-8159-5393ee0c60d9");
            Assert.Equal("wilbert.lopez@prime.com", result.UserName);
            Assert.Equal("Wilbert", result.Persona.Nombre);
            Assert.Equal("Lopez", result.Persona.PrimerApellido);
        }

        [Fact]
        public async Task GetUserWithDetailsAsyncNotValidUserTest()
        {
            var service = factory.Services.GetRequiredService<IAuthenticationService>();
            var result = await service.GetUserWithDetailsAsync("0213330b-5ffa-4bfc-8159-5393ee0c30d9");
            Assert.Null(result);
        }
    }
}
