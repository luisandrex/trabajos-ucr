using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Implementations.UserAdministration
{
    public class AuthenticationService : IAuthenticationService
    {
        protected readonly IAuthenticationRepository authenticationRepository;

        public AuthenticationService(IAuthenticationRepository _authenticationRepository)
        {
            authenticationRepository = _authenticationRepository;
        }

        public async Task<List<Perfil>> GetAllProfilesWithDetailsAsync()
        {
            return await authenticationRepository.GetPerfilesWithDetailsAsync();
        }

        public async Task<List<Usuario>> GetAllUsersWithDetailsAsync()
        {
            return await authenticationRepository.GetAllUsersWithDetailsAsync();
        }

        public async Task<Usuario> GetUserByEmailAsync(string email)
        {
            return await authenticationRepository.GetUserByEmailAsync(email);
        }

        public async Task<Usuario> GetUserWithDetailsAsync(string id)
        {
            return await authenticationRepository.GetWithDetailsAsync(id);
        }
    }
}
