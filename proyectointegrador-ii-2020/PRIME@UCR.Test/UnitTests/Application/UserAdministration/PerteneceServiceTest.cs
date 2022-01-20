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
    public class PerteneceServiceTest
    {
        [Fact]
        public async Task InsertUserProfileAsyncTest()
        {
            var userId = "155830367311";
            var profileId = "Administrador";

            var mockRepo = new Mock<IPerteneceRepository>();
            mockRepo.Setup(p => p.InsertUserToProfileAsync(userId, profileId));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var perteneceService = new SecurePerteneceService(mockRepo.Object, mockSecurity.Object);
            await perteneceService.InsertUserOfProfileAsync(userId, profileId);
        }

        [Fact]
        public async Task DeleteUserProfileAsyncTest()
        {
            var userId = "155830367311";
            var profileId = "Administrador";

            var mockRepo = new Mock<IPerteneceRepository>();
            mockRepo.Setup(p => p.DeleteUserFromProfileAsync(userId, profileId));

            var mockSecurity = new Mock<IPrimeSecurityService>();
            mockSecurity.Setup(s => s.CheckIfIsAuthorizedAsync(It.IsAny<AuthorizationPermissions[]>()));

            var perteneceService = new SecurePerteneceService(mockRepo.Object, mockSecurity.Object);
            await perteneceService.DeleteUserOfProfileAsync(userId, profileId);
        }

    }
}
