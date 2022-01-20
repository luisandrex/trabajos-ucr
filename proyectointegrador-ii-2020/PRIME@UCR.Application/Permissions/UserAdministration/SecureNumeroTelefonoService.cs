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
    public class SecureNumeroTelefonoService : INumeroTelefonoService
    {
        private readonly INumeroTelefonoRepository numeroTelefonoRepository;

        private readonly IPrimeSecurityService primeSecurityService;

        private readonly NumeroTelefonoService numeroTelefonoService;

        public SecureNumeroTelefonoService(INumeroTelefonoRepository _numeroTelefonoRepository,
            IPrimeSecurityService _primeSecurityService)
        {
            numeroTelefonoRepository = _numeroTelefonoRepository;
            primeSecurityService = _primeSecurityService;
            numeroTelefonoService = new NumeroTelefonoService(numeroTelefonoRepository);
        }

        public async Task<int> AddNewPhoneNumberAsync(string idUser, string phoneNumber)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            return await numeroTelefonoService.AddNewPhoneNumberAsync(idUser, phoneNumber);
        }
    }
}
