using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.Incidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.Incidents
{
    public class IncidentServiceIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;

        public IncidentServiceIntegrationTest(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllIncidentsReturnsNotEmpty() 
        {
            /* Case: There are incidents in post deployment
             * -> the list of all incidents wont be empty.
             */
            var incidentService = _factory.Services.GetRequiredService<IIncidentService>();
            var result = await incidentService.GetAllAsync();
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetIncidentAsyncReturnsNull()
        {
            /*
             * Case: There are incidents in post deployment
             * -> returns a null incident for the respective code that doesnt exist.
             */
            var incidentService = _factory.Services.GetRequiredService<IIncidentService>();
            var incidentCode = "Prueba";
            var result = await incidentService.GetIncidentAsync(incidentCode);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetIncidentAsyncReturnsNotNull()
        {
            /*
             * Case: There are incidents in post deployment
             * -> returns an incident with the respective code.
             */
            var incidentService = _factory.Services.GetRequiredService<IIncidentService>();
            var incidentCode = DateTime.Now.ToString("yyyy-MM-dd") + "-0001-IT-TER";        //Requires an existent incident
            var result = await incidentService.GetIncidentAsync(incidentCode);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetIncidentStateReturnsValid()
        {
            /*
             * Case: There are incidents in post deployment
             * -> returns an incident state for the respective code.
             */
            var incidentService = _factory.Services.GetRequiredService<IIncidentService>();
            var incidentCode = DateTime.Now.ToString("yyyy-MM-dd") + "-0001-IT-TER";        //Requires an existent incident
            var result = await incidentService.GetIncidentStateByIdAsync(incidentCode);
            Assert.NotNull(result);
        }
    }
}
