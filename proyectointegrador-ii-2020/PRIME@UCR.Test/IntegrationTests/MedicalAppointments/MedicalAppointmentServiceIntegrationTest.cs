using Microsoft.Extensions.DependencyInjection;
using PRIME_UCR.Application.Services.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PRIME_UCR.Test.IntegrationTests.MedicalAppointments
{
    public class MedicalAppointmentServiceIntegrationTest : IClassFixture<IntegrationTestWebApplicationFactory<Startup>>
    {

        private readonly IntegrationTestWebApplicationFactory<Startup> _factory;
        private readonly IAppointmentService appointment_service;



        public MedicalAppointmentServiceIntegrationTest(IntegrationTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            appointment_service = _factory.Services.GetRequiredService<IAppointmentService>();
        }


        [Fact]
        public async Task GetAppointmentByKeyNotValid()
        {
            var result = await appointment_service.GetAppointmentByKeyAsync(-1);
            Assert.Null(result); 
        }

        [Fact]
        public async Task GetMediStatusByNotValidId() {

            var result = await appointment_service.GetStatusById(-1);
            Assert.Null(result); 
        }

        [Fact]

        public async Task GetMedicalAppointmentWithAppointmentByNotValidKey() {


            var result = await appointment_service.GetMedicalAppointmentWithAppointmentByKeyAsync(-1);
            Assert.Null(result);

        }


        [Fact]
        public async Task GetDrugByConditionNotValidInputs() {

            var result = await appointment_service.GetDrugByConditionAsync(-1, -1);
            Assert.Null(result);

        }
    }
}
