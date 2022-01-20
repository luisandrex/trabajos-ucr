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
    public class SecurePermisoRepository : IPermisoRepository
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly PermisoRepository permisoRepository;

        public SecurePermisoRepository(ISqlDataProvider dataProvider,
            IPrimeSecurityService _primeSecurityService)
        {
            primeSecurityService = _primeSecurityService;
            permisoRepository = new PermisoRepository(dataProvider);
        }

        public async Task<List<Permiso>> GetAllAsync()
        {
            return await permisoRepository.GetAllAsync();
        }
    }
}
