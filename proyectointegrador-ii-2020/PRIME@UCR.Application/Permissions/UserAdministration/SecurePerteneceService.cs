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

    public class SecurePerteneceService : IPerteneceService
    {
        private readonly IPerteneceRepository perteneceRepository;

        private readonly IPrimeSecurityService primeSecurityService;

        private readonly PerteneceService perteneceService;

        public SecurePerteneceService(IPerteneceRepository _perteneceRepository,
            IPrimeSecurityService _primeSecurityService)
        {
            perteneceRepository = _perteneceRepository;
            primeSecurityService = _primeSecurityService;
            perteneceService = new PerteneceService(perteneceRepository);
        }

        public async Task DeleteUserOfProfileAsync(string idUser, string idProfile)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            await perteneceService.DeleteUserOfProfileAsync(idUser, idProfile);
        }

        public async Task InsertUserOfProfileAsync(string idUser, string idProfile)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            await perteneceService.InsertUserOfProfileAsync(idUser, idProfile);
        }
    }

}
