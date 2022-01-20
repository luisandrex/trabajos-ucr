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
    public class SecureEspecialistaTécnicoMédicoRepository : IEspecialistaTécnicoMédicoRepository
    {
        private readonly EspecialistaTécnicoMédicoRepository especialistaTécnicoMédicoRepository;

        private readonly IPrimeSecurityService primeSecurityService;

        public SecureEspecialistaTécnicoMédicoRepository(ISqlDataProvider dataProvider,
            IPrimeSecurityService _primeSecurityService)
        {
            primeSecurityService = _primeSecurityService;
            especialistaTécnicoMédicoRepository = new EspecialistaTécnicoMédicoRepository(dataProvider);
        }

        public async Task DeleteAsync(string key)
        {
            await especialistaTécnicoMédicoRepository.DeleteAsync(key);
        }

        public async Task<IEnumerable<EspecialistaTécnicoMédico>> GetAllAsync()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanAssignIncidents });
            return await especialistaTécnicoMédicoRepository.GetAllAsync();
        }

        public async Task<IEnumerable<EspecialistaTécnicoMédico>> GetByConditionAsync(Expression<Func<EspecialistaTécnicoMédico, bool>> expression)
        {
            return await especialistaTécnicoMédicoRepository.GetByConditionAsync(expression);
        }

        public async Task<EspecialistaTécnicoMédico> GetByKeyAsync(string key)
        {
            return await especialistaTécnicoMédicoRepository.GetByKeyAsync(key);
        }

        public async Task<EspecialistaTécnicoMédico> InsertAsync(EspecialistaTécnicoMédico model)
        {
            return await especialistaTécnicoMédicoRepository.InsertAsync(model);
        }

        public async Task UpdateAsync(EspecialistaTécnicoMédico model)
        {
            await especialistaTécnicoMédicoRepository.UpdateAsync(model);
        }
    }
}

