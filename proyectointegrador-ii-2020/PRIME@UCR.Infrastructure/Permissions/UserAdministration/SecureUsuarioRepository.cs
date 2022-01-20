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
    public class SecureUsuarioRepository : IUsuarioRepository
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly UsuarioRepository usuarioRepository;

        public SecureUsuarioRepository(ISqlDataProvider dataProvider, 
            IPrimeSecurityService _primeSecurityService)
        {
            primeSecurityService = _primeSecurityService;
            usuarioRepository = new UsuarioRepository(dataProvider);
        }

        public async Task<List<Usuario>> GetAllUsersWithDetailsAsync()
        {
            return await usuarioRepository.GetAllUsersWithDetailsAsync();
        }

        public async Task<List<Usuario>> GetNotAuthenticatedUsers()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            return await usuarioRepository.GetNotAuthenticatedUsers();
        }

        public async Task<Usuario> GetUserByEmailAsync(string email)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            return await usuarioRepository.GetUserByEmailAsync(email);
        }

        public async Task<Usuario> GetWithDetailsAsync(string id)
        {
            return await usuarioRepository.GetWithDetailsAsync(id);
        }

    }
}
