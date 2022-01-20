using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Constants;

namespace PRIME_UCR.Test.IntegrationTests
{
    public class MockSecurityService : IPrimeSecurityService
    {
        public Task CheckIfIsAuthorizedAsync(Type type, string methodName = null)
        {
            // do nothing
            return Task.CompletedTask;
        }

        public Task CheckIfIsAuthorizedAsync(AuthorizationPermissions[] permissions)
        {
            return Task.CompletedTask;
        }
    }
}
