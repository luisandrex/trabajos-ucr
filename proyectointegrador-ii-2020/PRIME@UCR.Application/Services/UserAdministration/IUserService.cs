using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Services.UserAdministration
{
    public interface IUserService
    {
        Task<List<Usuario>> GetAllUsersWithDetailsAsync();

        Task<Usuario> getUsuarioWithDetailsAsync(string id);

        Task<Persona> getPersonWithDetailstAsync(string email);

        Task<UserFormModel> GetUserFormFromRegisterUserFormAsync(RegisterUserFormModel userToRegister);

        Task<bool> StoreUserAsync(UserFormModel userToRegist);

        Task<List<Usuario>> GetNotAuthenticatedUsers();
    }
}
