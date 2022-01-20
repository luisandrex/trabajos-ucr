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
    public class SecurePersonaRepository : IPersonaRepository
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly PersonaRepository personaRepository;

        public SecurePersonaRepository(ISqlDataProvider dataProvider,
            IPrimeSecurityService _primeSecurityService)
        {
            primeSecurityService = _primeSecurityService;
            personaRepository = new PersonaRepository(dataProvider);
        }

        public async Task<Persona> GetByCedPersonaAsync(string ced)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            return await personaRepository.GetByCedPersonaAsync(ced);
        }
        
        public async Task DeleteAsync(string cedPersona)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            await personaRepository.DeleteAsync(cedPersona);
        }

        public async Task<Persona> GetByKeyPersonaAsync(string id)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanSeeBasicDetailsOfIncidents });
            return await personaRepository.GetByKeyPersonaAsync(id);
        }

        public async Task<Persona> GetWithDetailsAsync(string id)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            return await personaRepository.GetWithDetailsAsync(id);
        }

        public async Task InsertAsync(Persona persona)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanManageUsers });
            await personaRepository.InsertAsync(persona);
        }
    }
}
