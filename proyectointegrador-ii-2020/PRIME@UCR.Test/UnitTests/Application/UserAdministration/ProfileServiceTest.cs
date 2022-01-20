using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Permissions.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;
using Xunit;

namespace PRIME_UCR.Test.UnitTests.Application.UserAdministration
{
    public class ProfileServiceTest
    {
        [Fact]
        public async Task GetPerfilesWithDetailsAsyncTestEmpty()
        {
            var mockRepo = new Mock<IPerfilRepository>();
            mockRepo.Setup(u => u.GetPerfilesWithDetailsAsync()).Returns(Task.FromResult(new List<Perfil>()));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var profileService = new SecureProfilesService(mockSecurity.Object, mockRepo.Object);
            var result = await profileService.GetPerfilesWithDetailsAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPerfilesWithDetailsAsyncTestNotEmpty()
        {
            var mockRepo = new Mock<IPerfilRepository>();
            mockRepo.Setup(u => u.GetPerfilesWithDetailsAsync()).Returns(Task.FromResult(new List<Perfil>()
            {
                new Perfil()
                {
                    NombrePerfil = "Administrador",
                },
                new Perfil()
                { 
                    NombrePerfil = "Médico",
                }

            })) ;

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var profileService = new SecureProfilesService(mockSecurity.Object, mockRepo.Object);
            var result = await profileService.GetPerfilesWithDetailsAsync();
            Assert.Equal(2, result.Count);
        }
    }
}
