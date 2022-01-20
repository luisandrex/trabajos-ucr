using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.UserAdministration
{
    public class NumeroTelefonoIntegrationTests : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        public NumeroTelefonoIntegrationTests(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        [Fact]
        private async Task AddNewPhoneNumberAsyncReturnsOne()
        {
            var numeroTelefonoService = _factory.Services.GetRequiredService<INumeroTelefonoService>();
            var result = await numeroTelefonoService.AddNewPhoneNumberAsync("23456789", "85312773");
            Assert.Equal(1, result);
        }
    }
}
