using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Services.UserAdministration
{
    public interface IAuthenticationService
    {
        Task<List<Usuario>> GetAllUsersWithDetailsAsync();
        Task<List<Perfil>> GetAllProfilesWithDetailsAsync();
        Task<Usuario> GetUserWithDetailsAsync(string id);
        Task<Usuario> GetUserByEmailAsync(string email);
    }
}
