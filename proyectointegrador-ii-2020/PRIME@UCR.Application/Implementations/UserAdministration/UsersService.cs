using Microsoft.AspNetCore.Identity;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Permissions.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Implementations.UserAdministration
{
    internal class UsersService : IUserService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        private readonly UserManager<Usuario> userManager;

        public UsersService(
            IUsuarioRepository usuarioRepository,
            UserManager<Usuario> _userManager)
        {
            _usuarioRepository = usuarioRepository;
            userManager = _userManager;
        }

        /**
         * Method used to get the person info by its email
         */
        public async Task<Persona> getPersonWithDetailstAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var person = await getUsuarioWithDetailsAsync(user?.Id);
                return person?.Persona;
            }
            return null;
        }

        /**
         * Method used to get an user DTO from a register form DTO
         *
         * Return: A user DTO with the info of the user.
         */
        public Task<UserFormModel> GetUserFormFromRegisterUserFormAsync(RegisterUserFormModel userToRegister)
        {
            if(userToRegister != null)
            {
                UserFormModel userModel = new UserFormModel();
                userModel.Email = userToRegister.Email;
                userModel.IdCardNumber = userToRegister.IdCardNumber;
                return Task.FromResult(userModel);
            }
            return null; 
        }

        /**
         * Method used to get all the info of a user given its email.
         * 
         * Return: All the info of the user.
         */
        public async Task<Usuario> getUsuarioWithDetailsAsync(string id)
        {
            if (id == null || id == string.Empty)
            {
                return null;
            }
            return await _usuarioRepository.GetWithDetailsAsync(id);
        }

        /**
         * Method used to get a user from a user DTO.
         * 
         * Return: A user with the info given in the DTO.
         */
        public Usuario GetUserFromUserModel(UserFormModel userToRegister)
        {
            Usuario user = new Usuario();
            user.Email = user.UserName = userToRegister.Email;
            return user;
        }

        /**
         * Method used to store a user in the database given all the necessary info of the new user.
         */
        public async Task<bool> StoreUserAsync(UserFormModel userToRegist)
        {
            var user = GetUserFromUserModel(userToRegist);
            var existInDB = (await userManager.FindByEmailAsync(user.Email)) == null ? false : true;
            if (!existInDB)
            {
                user.CedPersona = userToRegist.IdCardNumber;
                var result = await userManager.CreateAsync(user);
                return result.Succeeded;
            }
            else
            {
                return false;
            }
        }

        /**
         * Method used to get all the users with details.
         * 
         * Return: A list with all the users with the details.
         */
        public async Task<List<Usuario>> GetAllUsersWithDetailsAsync()
        {
            return await _usuarioRepository.GetAllUsersWithDetailsAsync();
        }


        /**
         * Method used to get all the users that aren't validated yet.
         * 
         * Return: A list with all the users that have not been validated in the app.
         */
        public async Task<List<Usuario>> GetNotAuthenticatedUsers()
        {
            return await _usuarioRepository.GetNotAuthenticatedUsers();
        }
    }
}
