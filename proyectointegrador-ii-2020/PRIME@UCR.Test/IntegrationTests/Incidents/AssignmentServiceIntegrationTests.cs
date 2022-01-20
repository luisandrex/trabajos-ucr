using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.Incidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.Incidents
{
    public class AssignmentServiceIntegrationTests : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;

        public AssignmentServiceIntegrationTests(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllTransportUnitsByModeReturnsEmpty()
        {

            var assignementService = _factory.Services.GetRequiredService<IAssignmentService>();
            var result = await assignementService.GetAllTransportUnitsByMode(null);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllTransportUnitsByModeReturnsValid()
        {

            var assignementService = _factory.Services.GetRequiredService<IAssignmentService>();
            var result = await assignementService.GetAllTransportUnitsByMode("Terrestre");
            Assert.Collection(result,
                                  unidad => Assert.Equal("BPC086", unidad.Matricula),
                                  unidad => Assert.Equal("FMM420", unidad.Matricula)
                                  );
        }

        [Fact]
        public async Task GetAllTransportUnitsByModeReturnsInvalid()
        {

            var assignementService = _factory.Services.GetRequiredService<IAssignmentService>();
            var result = await assignementService.GetAllTransportUnitsByMode("Invalid unit");
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCoordinatorsAsyncReturnsValid()
        {
            /* Case: The service should return the exact same number of Medical Coordinators
             *  that was written on the post deployment script
             * -> Returns: A list with the coordinators found by the repository.
            */
            int numberOfCordinatorsinDB = 2;
            var assignementService = _factory.Services.GetRequiredService<IAssignmentService>();
            var result = await assignementService.GetCoordinatorsAsync();
            var list = result.ToList();
            Assert.Equal(numberOfCordinatorsinDB, list.Count);
        }

        [Fact]
        public async Task GetAssignedOriginDoctorReturnsNull()
        {
            /* Case: The service should return the assigned doctor in the origin of the incident.
             *  in this case, the incident's origin is a household thus it does not have an
             *  origin doctor assigned.
             * -> Returns: Null
            */
            var incidentCode = "2020-11-16-0001-IT-TER";
            var assignementService = _factory.Services.GetRequiredService<IAssignmentService>();
            var result = await assignementService.GetAssignedOriginDoctor(incidentCode);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAssignedDestinationDoctorReturnsNull()
        {
            /* Case: The service should return the assigned doctor in the destination of the incident.
             *  in this case, the destination has not been assigned yet. Thus the test should return
             *  null.
             * -> Returns: Null
            */
            var incidentCode = "2020-11-16-0001-IT-TER";
            var assignementService = _factory.Services.GetRequiredService<IAssignmentService>();
            var result = await assignementService.GetAssignedDestinationDoctor(incidentCode);
            Assert.Null(result);
        }

    }
}
