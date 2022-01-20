using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Permissions.UserAdministration
{
    public class SecureDoctorRepository : IDoctorRepository
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly DoctorRepository doctorRepository;

        public SecureDoctorRepository(ISqlDataProvider dataProvider,
            IPrimeSecurityService _primeSecurityService)
        {
            doctorRepository = new DoctorRepository(dataProvider);
            primeSecurityService = _primeSecurityService;
        }

        public async Task<Médico> GetByKeyAsync(string key)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanAssignIncidents });
            return await doctorRepository.GetByKeyAsync(key);
        }

        public async Task<IEnumerable<Médico>> GetAllAsync()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanAssignIncidents });
            return await doctorRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Médico>> GetByConditionAsync(Expression<Func<Médico, bool>> expression)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanAssignIncidents });
            return await doctorRepository.GetByConditionAsync(expression);
        }

        public async Task<Médico> InsertAsync(Médico model)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanAssignIncidents });
            return await doctorRepository.InsertAsync(model);
        }

        public async Task DeleteAsync(string key)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanAssignIncidents });
            await doctorRepository.DeleteAsync(key);
        }

        public async Task UpdateAsync(Médico model)
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanAssignIncidents });
            await doctorRepository.UpdateAsync(model);
        }
    }
}

