using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.UserAdministration
{
    public class PermiteServiceIntegrationTests : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        public PermiteServiceIntegrationTests(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async Task InsertPermissionValid()
        {
            string idProfile = "Médico";
            int idPermission = 1;

            var permiteService = _factory.Services.GetRequiredService<IPermiteService>();
            await permiteService.InsertPermissionAsync(idProfile, idPermission);

        }
        [Fact]
        public async Task DeletePermissionValid()
        {
            string idProfile = "Administrador";
            int idPermission = 12;

            var permiteService = _factory.Services.GetRequiredService<IPermiteService>();
            await permiteService.DeletePermissionAsync(idProfile, idPermission);
        }
    }
}

/*[Fact]
public async Task InsertPermissionAsyncTest()
{
    string idProfile = "Administrador";
    int idPermission = 12;

    var mockRepo = new Mock<IPermiteRepository>();
    mockRepo.Setup(p => p.InsertPermissionAsync(idProfile, idPermission));

    var mockSecurity = new Mock<IPrimeSecurityService>();
    mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(typeof(PermiteService), "InsertPermissionAsync"));

    var permiteService = new PermiteService(mockRepo.Object, mockSecurity.Object);
    await permiteService.InsertPermissionAsync(idProfile, idPermission);
}

[Fact]
public async Task DeletePermissionAsyncTest()
{
    string idProfile = "Administrador";
    int idPermission = 12;

    var mockRepo = new Mock<IPermiteRepository>();
    mockRepo.Setup(p => p.DeletePermissionAsync(idProfile, idPermission));

    var mockSecurity = new Mock<IPrimeSecurityService>();
    mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(typeof(PermiteService), "InsertPermissionAsync"));

    var permiteService = new PermiteService(mockRepo.Object, mockSecurity.Object);
    await permiteService.DeletePermissionAsync(idProfile, idPermission);
}*/