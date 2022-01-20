using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using PRIME_UCR.Application.Repositories.UserAdministration;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.Permissions.UserAdministration;
using PRIME_UCR.Domain.Constants;

namespace PRIME_UCR.Test.UnitTests.Application.UserAdministration
{
    public class NumeroTelefonicoServiceTest
    {
        //        Task AddNewPhoneNumberAsync(string idUser, string phoneNumber);
        [Fact]
        public async Task AddNewPhoneNumberTest()
        {

            NúmeroTeléfono userPhoneNumber = new NúmeroTeléfono();
            userPhoneNumber.CedPersona = "117980341";
            userPhoneNumber.NúmeroTelefónico = "84312773";

            var mockRepo = new Mock<INumeroTelefonoRepository>();
            mockRepo.Setup(p => p.AddPhoneNumberAsync(It.IsAny<NúmeroTeléfono>())).Returns(Task.FromResult(1));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var phoneNumberService = new SecureNumeroTelefonoService(mockRepo.Object, mockSecurity.Object);
            var result = await phoneNumberService.AddNewPhoneNumberAsync("117980341", "84312773");

            Assert.Equal(1, result);
        }
    }
}
