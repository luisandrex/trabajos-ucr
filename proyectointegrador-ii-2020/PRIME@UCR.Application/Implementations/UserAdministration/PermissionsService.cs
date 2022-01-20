using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Implementations.UserAdministration
{
    internal class PermissionsService : IPermissionsService
    {
        private readonly IPermisoRepository _permissionsRepository;

        public PermissionsService(IPermisoRepository permisoRepository)
        {
            _permissionsRepository = permisoRepository;
        }

        public async Task<IEnumerable<Permiso>> GetPermisos()
        {
            return await _permissionsRepository.GetAllAsync();
        }
    }
}   
