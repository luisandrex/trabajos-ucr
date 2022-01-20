using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.MedicalAppointments
{
    public class PrescriptionServiceIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {

        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        private readonly IAppointmentService appointment_service;


        public PrescriptionServiceIntegrationTest(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            appointment_service = _factory.Services.GetRequiredService<IAppointmentService>();
        }

        [Fact]
        public async Task GetPrescriptionsByAppointmentIdNotValid()
        {
            var result = await appointment_service.GetPrescriptionsByAppointmentId(-1);
            Assert.Empty(result);
        }


        [Fact]
        public async Task GetAllAsync()
        {
            var result = await appointment_service.GetDrugsAsync();
            //Asserts the result
            Assert.Equal(10, result.Count());
        }



    }
}
