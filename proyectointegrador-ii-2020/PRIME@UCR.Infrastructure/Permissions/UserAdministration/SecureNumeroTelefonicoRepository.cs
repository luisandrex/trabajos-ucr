using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Permissions.UserAdministration
{
    public class SecureNumeroTelefonicoRepository : INumeroTelefonoRepository
    {
        private readonly NumeroTelefonoRepository numeroTelefonoRepository;

        private readonly IPrimeSecurityService primeSecurityService;

        public SecureNumeroTelefonicoRepository(ISqlDataProvider dataProvider,
            IPrimeSecurityService _primeSecurityService)
        {
            primeSecurityService = _primeSecurityService;
            numeroTelefonoRepository = new NumeroTelefonoRepository(dataProvider);
        }

        public async Task<int> AddPhoneNumberAsync(NúmeroTeléfono phoneNumber)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            return await numeroTelefonoRepository.AddPhoneNumberAsync(phoneNumber);
        }
    }
}
