using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Implementations.UserAdministration;
using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Permissions.UserAdministration
{

    public class SecurePersonService : IPersonService
    {
        private readonly IPersonaRepository PersonRepository;

        private readonly IPrimeSecurityService primeSecurityService;

        private readonly PersonService personService;

        public SecurePersonService(IPersonaRepository _personaRepository,
            IPrimeSecurityService _primeSecurityService)
        {
            PersonRepository = _personaRepository;
            primeSecurityService = _primeSecurityService;
            personService = new PersonService(PersonRepository);
        }

        public async Task<Persona> GetPersonByCedAsync(string ced)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            return await personService.GetPersonByCedAsync(ced);
        }

        public async Task<Persona> GetPersonByIdAsync(string id)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeBasicDetailsOfIncidents });
            return await personService.GetPersonByIdAsync(id);
        }

        public async Task<PersonFormModel> GetPersonModelFromRegisterModelAsync(RegisterUserFormModel registerUserModel)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            return await personService.GetPersonModelFromRegisterModelAsync(registerUserModel);
        }

        public async Task StoreNewPersonAsync(PersonFormModel personInfo)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            await personService.StoreNewPersonAsync(personInfo);
        }

        public async Task DeletePersonAsync(string id)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            await personService.DeletePersonAsync(id);
        }
    }
}
