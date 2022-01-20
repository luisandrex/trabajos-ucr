using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.MedicalRecord
{
    public class MedicalRecordServiceIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {
        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        private readonly IMedicalRecordService mrService;

        public MedicalRecordServiceIntegrationTest(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            mrService = _factory.Services.GetRequiredService<IMedicalRecordService>();
        }

        [Fact]
        public async Task GetAllNotNull()
        {
            var result = await mrService.GetAll();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByPatientIdAsyncNull()
        {
            var result = await mrService.GetByPatientIdAsync("000000000");
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByPatientIdAsyncValid()
        {
            var result = await mrService.GetByPatientIdAsync("12345678");
            Assert.NotNull(result);
        }
    }
}
