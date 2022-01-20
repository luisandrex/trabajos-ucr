using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Infrastructure.DataProviders;
using PRIME_UCR.Infrastructure.Repositories.Sql;
using PRIME_UCR.Infrastructure.Repositories.Sql.Incidents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Permissions.Incidents
{
    public class SecureStateRepository : RepoDbRepository<Estado, string>, IStateRepository
    {
        private readonly StateRepository stateRepository;

        private readonly IPrimeSecurityService primeSecurityService;

        public SecureStateRepository(ISqlDataProvider sqlDataProvider,
            IPrimeSecurityService _primeSecurityService) : base(sqlDataProvider)
        {
            primeSecurityService = _primeSecurityService;
            stateRepository = new StateRepository(sqlDataProvider);
        } 

        public async Task<IEnumerable<Estado>> GetAllStates()
        {
            return await stateRepository.GetAllStates();
        }
    }
}
