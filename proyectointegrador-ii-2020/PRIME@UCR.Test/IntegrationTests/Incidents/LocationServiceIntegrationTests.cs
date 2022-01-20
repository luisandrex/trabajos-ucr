using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Domain.Models;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.Incidents
{
    public class LocationServiceIntegrationTests : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;

        public LocationServiceIntegrationTests(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetCountryByNameReturnsNull()
        {
            var locationService = _factory.Services.GetRequiredService<ILocationService>();
            var result = await locationService.GetCountryByName(null);
            Assert.Null(result);
        }


        [Fact]
        public async Task GetCountryByNameReturnsInvalid()
        {
            var locationService = _factory.Services.GetRequiredService<ILocationService>();
            var result = await locationService.GetCountryByName("invalid country");
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCountryByNameReturnsValid()
        {
            var locationService = _factory.Services.GetRequiredService<ILocationService>();
            var result = await locationService.GetCountryByName("Costa Rica");
            Assert.Equal(new Pais{ Nombre = "Costa Rica"}, result);
        }
    }
}
