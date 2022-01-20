using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Exceptions.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace PRIME_UCR.Application.Implementations.UserAdministration
{
    public class PrimeSecurityService : IPrimeSecurityService
    {
        protected readonly IAuthorizationService authorizationService;

        protected readonly AuthenticationStateProvider authenticationStateProvider;

        public PrimeSecurityService(IAuthorizationService _authorizationService,
            AuthenticationStateProvider _authenticationStateProvider)
        {
            authorizationService = _authorizationService;
            authenticationStateProvider = _authenticationStateProvider;
        }

        public async Task CheckIfIsAuthorizedAsync(AuthorizationPermissions[] permissions)
        {
            var user = (await authenticationStateProvider.GetAuthenticationStateAsync()).User;
            var isAuthorized = true;
            AuthorizationPermissions missingPermission = default;
            foreach (var permission in permissions)
            {
                if (!(await authorizationService.AuthorizeAsync(user, permission.ToString())).Succeeded)
                {
                    isAuthorized = false;
                    missingPermission = permission;
                    break;
                }

            }
            if (!isAuthorized)
            {
                throw new NotAuthorizedException($"Missing the following permission: {missingPermission.ToString()}");
            }
        }
    }
}
