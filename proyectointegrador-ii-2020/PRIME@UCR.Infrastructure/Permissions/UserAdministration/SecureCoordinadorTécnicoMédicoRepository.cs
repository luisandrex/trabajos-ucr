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
    public class SecureCoordinadorTécnicoMédicoRepository : ICoordinadorTécnicoMédicoRepository
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly CoordinadorTécnicoMédicoRepository coordinadorTécnicoMédico;

        public SecureCoordinadorTécnicoMédicoRepository(ISqlDataProvider dataProvider,
            IPrimeSecurityService _primeSecurityService)
        {
            primeSecurityService = _primeSecurityService;
            coordinadorTécnicoMédico = new CoordinadorTécnicoMédicoRepository(dataProvider);
        }

        public async Task<CoordinadorTécnicoMédico> GetByKeyAsync(string key)
        {
            return await coordinadorTécnicoMédico.GetByKeyAsync(key);
        }

        public async Task<IEnumerable<CoordinadorTécnicoMédico>> GetAllAsync()
        {
            return await coordinadorTécnicoMédico.GetAllAsync();
        }

        public async Task<IEnumerable<CoordinadorTécnicoMédico>> GetByConditionAsync(Expression<Func<CoordinadorTécnicoMédico, bool>> expression)
        {
            return await coordinadorTécnicoMédico.GetByConditionAsync(expression);
        }

        public async Task<CoordinadorTécnicoMédico> InsertAsync(CoordinadorTécnicoMédico model)
        {
            return await coordinadorTécnicoMédico.InsertAsync(model);
        }

        public async Task DeleteAsync(string key)
        {
            await coordinadorTécnicoMédico.DeleteAsync(key);
        }

        public async Task UpdateAsync(CoordinadorTécnicoMédico model)
        {
            await coordinadorTécnicoMédico.UpdateAsync(model);
        }
    }
}
