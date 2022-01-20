using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Application.Permissions.UserAdministration
{ 
    
    public class SecureUserService : IUserService
    {
        private readonly IUsuarioRepository usuarioRepository;

        private readonly UserManager<Usuario> userManager;

        private readonly IPrimeSecurityService primeSecurityService;

        private readonly UsersService userService;

        public SecureUserService(IUsuarioRepository _usuarioRepository,
            UserManager<Usuario> _userManager,
            IPrimeSecurityService _primeSecurityService)
        {
            userManager = _userManager;
            usuarioRepository = _usuarioRepository;
            primeSecurityService = _primeSecurityService;
            userService = new UsersService(usuarioRepository, userManager);
        }

        public async Task<List<Usuario>> GetAllUsersWithDetailsAsync()
        {
            return await userService.GetAllUsersWithDetailsAsync();
        }
        
        public async Task<List<Usuario>> GetNotAuthenticatedUsers()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            return await userService.GetNotAuthenticatedUsers();
        }

        public async Task<Persona> getPersonWithDetailstAsync(string email)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeBasicDetailsOfIncidents });
            return await userService.getPersonWithDetailstAsync(email);
        }

        public async Task<UserFormModel> GetUserFormFromRegisterUserFormAsync(RegisterUserFormModel userToRegister)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            if(userToRegister != null)
            {
                return await userService.GetUserFormFromRegisterUserFormAsync(userToRegister);
            } else
            {
                return await Task.FromResult<UserFormModel>(null);
            }
        }

        public async Task<Usuario> getUsuarioWithDetailsAsync(string id)
        {
            return await userService.getUsuarioWithDetailsAsync(id);
        }

        public async Task<bool> StoreUserAsync(UserFormModel userToRegist)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            return await userService.StoreUserAsync(userToRegist);
        }
    }
}
