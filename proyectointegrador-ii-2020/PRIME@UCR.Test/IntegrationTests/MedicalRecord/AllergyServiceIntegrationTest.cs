using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.MedicalRecord
{
    public class AllergyServiceIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        private readonly IAlergyService aService;

        public AllergyServiceIntegrationTest(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            aService = _factory.Services.GetRequiredService<IAlergyService>();
        }

        [Fact]
        public async Task GetAlergyByRecordIdEmpty()
        {
            var result = await aService.GetAlergyByRecordId(-1);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAlergyByRecordId()
        {
            var result = await aService.GetAlergyByRecordId(12345678);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync()
        {
            var result = await aService.GetAll();
            //Asserts the result
            Assert.Equal(3, result.Count());
        }

    }
}
