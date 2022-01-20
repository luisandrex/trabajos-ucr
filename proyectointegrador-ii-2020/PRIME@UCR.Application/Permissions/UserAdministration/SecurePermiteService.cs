using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Permissions.UserAdministration
{
    public class SecurePermiteService : IPermiteService
    {
        private readonly IPermiteRepository permiteRepository;

        private readonly IPrimeSecurityService primeSecurityService;

        private readonly PermiteService permiteService;

        public SecurePermiteService(IPermiteRepository _permiteRepository,
            IPrimeSecurityService _primeSecurityService)
        {
            permiteRepository = _permiteRepository;
            primeSecurityService = _primeSecurityService;
            permiteService = new PermiteService(permiteRepository);
        }

        public async Task DeletePermissionAsync(string idProfile, int idPermission)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            await permiteService.DeletePermissionAsync(idProfile, idPermission);
        }

        public async Task InsertPermissionAsync(string idProfile, int idPermission)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            await permiteService.InsertPermissionAsync(idProfile, idPermission);
        }
    }
}
