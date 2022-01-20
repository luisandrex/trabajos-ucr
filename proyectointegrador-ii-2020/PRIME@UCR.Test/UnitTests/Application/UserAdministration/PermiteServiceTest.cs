using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Permissions.UserAdministration;
using PRIME_UCR.Domain.Constants;

namespace PRIME_UCR.Test.UnitTests.Application.UserAdministration
{
    public class PermiteServiceTest
    {
        [Fact]
        public async Task InsertPermissionAsyncTest()
        {
            string idProfile = "Administrador";
            int idPermission = 12;   

            var mockRepo = new Mock<IPermiteRepository>();
            mockRepo.Setup(p => p.InsertPermissionAsync(idProfile,idPermission));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var permiteService = new SecurePermiteService(mockRepo.Object, mockSecurity.Object);
            await permiteService.InsertPermissionAsync(idProfile,idPermission);
        }

        [Fact]
        public async Task DeletePermissionAsyncTest()
        {
            string idProfile = "Administrador";
            int idPermission = 12;

            var mockRepo = new Mock<IPermiteRepository>();
            mockRepo.Setup(p => p.DeletePermissionAsync(idProfile, idPermission));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var permiteService = new SecurePermiteService(mockRepo.Object, mockSecurity.Object);
            await permiteService.DeletePermissionAsync(idProfile, idPermission);
        }

    }
}