using System.Collections.Generic;
using Xunit;
using Moq;
using System.Threading.Tasks;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Application.Services.Appointments;
using PRIME_UCR.Application.Permissions.Appointments;

namespace PRIME_UCR.Test.UnitTests.Application.MedicalAppointments
{
    public class MedicalAppointmentServiceTest
    {
        [Fact]
        public async void GetStatusByIdNull()
        {
            var mockRepo = new Mock<IAppointmentStatusRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            mockRepo.Setup(s => s.GetByKeyAsync(0)).Returns(Task.FromResult(await mockRepo.Object.GetByKeyAsync(-1)));
            IAppointmentService appointmentService 
                        = new SecureAppointmentService(null, null, null, null, null, null, null, null, null, null, null,
                                mockRepo.Object, mockSecurity.Object);
            var result = await appointmentService.GetStatusById(0);
            Assert.Null(result);
        }

        [Fact] 
        public async void GetLastAppointmentDateAsyncNull() 
        {
            var mockRepo = new Mock<IAppointmentRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            mockRepo.Setup(s => s.getLatestAppointmentByRecordId(0)).Returns(Task.FromResult(await mockRepo.Object.getLatestAppointmentByRecordId(-1)));
            IAppointmentService appointmentService
                        = new SecureAppointmentService(null, mockRepo.Object, null, null, null, null, null, null, null, null, null,
                                null, mockSecurity.Object);
            var result = await appointmentService.GetLastAppointmentDateAsync(0);
            Assert.Null(result);
        }

        [Fact]
        public async void GetMedicalAppointmentByAppointmentId()
        {
            var mockRepo = new Mock<IMedicalAppointmentRepository>();
            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));
            mockRepo.Setup(s => s.GetByAppointmentId(0)).Returns(Task.FromResult(await mockRepo.Object.GetByAppointmentId(-1)));
            IAppointmentService appointmentService
                        = new SecureAppointmentService(null, null, null, mockRepo.Object, null, null, null, null, null, null, null,
                                null, mockSecurity.Object);
            var result = await appointmentService.GetMedicalAppointmentByAppointmentId(0);
            Assert.Null(result);
        }
    }
}
