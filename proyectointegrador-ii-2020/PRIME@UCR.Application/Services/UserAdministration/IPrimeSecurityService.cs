using PRIME_UCR.Application.DTOs.UserAdministration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Constants;
using System.Runtime.CompilerServices;

namespace PRIME_UCR.Application.Services.UserAdministration
{
    public interface IPrimeSecurityService
    {
        Task CheckIfIsAuthorizedAsync(AuthorizationPermissions[] permissions);
    }
}
