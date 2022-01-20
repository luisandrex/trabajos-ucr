using PRIME_UCR.Application.Implementations.Incidents;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.Incidents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Permissions.Incidents
{
    public class SecureStateService : IStateService
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly StateService stateService;

        public SecureStateService(IPrimeSecurityService _primeSecurityService,
            IStateRepository stateRepository)
        {
            primeSecurityService = _primeSecurityService;
            stateService = new StateService(stateRepository);
        }

        public async Task<IEnumerable<Estado>> GetAllStates()
        {
            return await stateService.GetAllStates();
        }

    }
}
