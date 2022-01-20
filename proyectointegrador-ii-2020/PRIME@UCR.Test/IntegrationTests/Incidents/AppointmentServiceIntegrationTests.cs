using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.Incidents;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.Incidents
{
    public class AppointmentServiceIntegrationTests : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        private readonly IAssignmentService _service;

        public AppointmentServiceIntegrationTests(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _service = _factory.Services.GetRequiredService<IAssignmentService>();
        }

        [Fact]
        public async Task IsAuthorizedToViewPatientWhenValid()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _service.IsAuthorizedToViewPatient(null, null));
        }
    }
}
