using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.MedicalRecord
{
    public class ChronicConditionIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        private readonly IChronicConditionService cdService;

        public ChronicConditionIntegrationTest(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            cdService = _factory.Services.GetRequiredService<IChronicConditionService>();
        }
        [Fact]
        public async Task GetChronicConditionByRecordIdy()
        {
            var result = await cdService.GetChronicConditionByRecordId(12345678);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetChronicConditionByRecordIdEmpty()
        {
            var result = await cdService.GetChronicConditionByRecordId(-1);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync()
        {
            var result = await cdService.GetAll();
            //Asserts the result
            Assert.Equal(4, result.Count());
        }
    }
}