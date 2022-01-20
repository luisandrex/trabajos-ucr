using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using PRIME_UCR.Application.Services.UserAdministration;

namespace PRIME_UCR.Test.IntegrationTests.UserAdministration
{
    public class PerfilServiceIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        
        public PerfilServiceIntegrationTest(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task InsertPermissionValid()
        {
            var perfilService = _factory.Services.GetRequiredService<IProfilesService>();
            var result = await perfilService.GetPerfilesWithDetailsAsync();
            Assert.Equal(6, result.Count);
            Assert.Equal(2, result[0].UsuariosYPerfiles.Count);
        }
    }
}
