using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.Services.Dashboard;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.Dashboards
{
    public class DashboardServiceIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;

        public DashboardServiceIntegrationTest(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllIncidentsAsyncReturnsNotEmpty()
        {
            /* Case: There are incidents in post deployment
             * -> the list of all incidents wont be empty.
             */
            var dashboardService = _factory.Services.GetRequiredService<IDashboardService>();
            var result = await dashboardService.GetAllIncidentsAsync();
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetAllDistrictsAsyncReturnsNotEmpty()
        {
            /* Case: There are districts in post deployment
             * -> the list of all districts wont be empty.
             */
            var dashboardService = _factory.Services.GetRequiredService<IDashboardService>();
            var result = await dashboardService.GetAllDistrictsAsync();
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetFilteredIncidentsListAsyncReturnsNotEmpty()
        {
            /* Case: There are incidents in post deployment and at least one incident have the requirements
             * of the Filter
             * -> the list of all incidents wont be empty.
             */
            var modalityFilter = new Modalidad();
            modalityFilter.Tipo = "Terrestre";
            var model = new FilterModel { ModalityFilter = modalityFilter };

            var dashboardService = _factory.Services.GetRequiredService<IDashboardService>();
            var allIncidents = await dashboardService.GetAllIncidentsAsync();
            var result = await dashboardService.GetFilteredIncidentsList(model);
            Assert.NotEmpty(result);
            Assert.True(allIncidents.Count() > result.Count());
        }

        [Fact]
        public async Task TaskGetIncidentCounterAsyncReturnsNotNull()
        {
            /* Case: There are incidents in post deployment and at least one incident have the requirements
             * of the Filter
             * -> the list of all incidents wont be empty.
             */
           //var modality = "Terrestre";

           // var dashboardService = _factory.Services.GetRequiredService<IDashboardService>();
           // var result = 0;
           // try
           // {
           //    result = await dashboardService.GetIncidentCounterAsync(modality);
           // }
           // catch (Exception e)
           // {
           //     var error = 1;
           // }
            
           // Assert.True(result >= 0);
        }

        

    }
}
