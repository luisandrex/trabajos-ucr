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
    public class SecurePerteneceRepository : IPerteneceRepository
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly PerteneceRepository perteneceRepository;

        public SecurePerteneceRepository(ISqlDataProvider dataProvider,
            IPrimeSecurityService _primeSecurityService)
        {
            primeSecurityService = _primeSecurityService;
            perteneceRepository = new PerteneceRepository(dataProvider);
        }

        public async Task DeleteUserFromProfileAsync(string idUser, string idProfile)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            await perteneceRepository.DeleteUserFromProfileAsync(idUser, idProfile);
        }

        public async Task InsertUserToProfileAsync(string idUser, string idProfile)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            await perteneceRepository.InsertUserToProfileAsync(idUser, idProfile);
        }
    }
}
