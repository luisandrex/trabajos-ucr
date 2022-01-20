using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.MedicalRecord
{
    public class MedicalBackgroundServiceIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        private readonly IMedicalBackgroundService mbService;

        public MedicalBackgroundServiceIntegrationTest(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            mbService = _factory.Services.GetRequiredService<IMedicalBackgroundService>();
        }

        [Fact]
        public async Task GetBackgroundByRecordId()
        {
            var result = await mbService.GetBackgroundByRecordId(12345678);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetBackgroundByRecordIdEmpty()
        {
            var result = await mbService.GetBackgroundByRecordId(-1);
            Assert.Empty(result);
        }
        [Fact]
        public async Task GetAllAsync()
        {
            var result = await mbService.GetAll();
            //Asserts the result
            Assert.Equal(4,result.Count());
        }
    }
}
