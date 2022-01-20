using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Infrastructure.DataProviders;
using PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Permissions.UserAdministration
{
    public class SecurePermiteRepository : IPermiteRepository
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly PermiteRepository permiteRepository;

        public SecurePermiteRepository(ISqlDataProvider dataProvider,
            IPrimeSecurityService _primeSecurityService)
        {
            primeSecurityService = _primeSecurityService;
            permiteRepository = new PermiteRepository(dataProvider);
        }

        public async Task DeletePermissionAsync(string idProfile, int idPermission)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            await permiteRepository.DeletePermissionAsync(idProfile, idPermission);
        }

        public async Task InsertPermissionAsync(string idProfile, int idPermission)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            await permiteRepository.InsertPermissionAsync(idProfile, idPermission);
        }
    }
}
