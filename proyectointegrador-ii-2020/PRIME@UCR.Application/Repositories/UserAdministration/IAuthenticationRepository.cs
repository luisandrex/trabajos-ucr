using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.UserAdministration
{
    public interface IAuthenticationRepository
    {
        Task<List<Perfil>> GetPerfilesWithDetailsAsync();

        Task<List<Usuario>> GetAllUsersWithDetailsAsync();

        Task<Usuario> GetUserByEmailAsync(string email);

        Task<Usuario> GetWithDetailsAsync(string id);
    }
}
