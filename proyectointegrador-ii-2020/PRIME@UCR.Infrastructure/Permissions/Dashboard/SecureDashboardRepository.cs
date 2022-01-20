using PRIME_UCR.Application.Repositories.Dashboard;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using PRIME_UCR.Infrastructure.Repositories.Sql.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Permissions.Dashboard
{
    public class SecureDashboardRepository : IDashboardRepository
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly DashboardRepository dashboardRepository;

        public SecureDashboardRepository(ISqlDataProvider sqlDataProvider,
            IPrimeSecurityService securityService)
        {
            primeSecurityService = securityService;
            dashboardRepository = new DashboardRepository(sqlDataProvider);
        }

        public async Task DeleteAsync(string key)
        {
            await dashboardRepository.DeleteAsync(key);
        }

        public async Task<IEnumerable<Incidente>> GetAllAsync()
        {
            return await dashboardRepository.GetAllAsync();
        }

        public async Task<List<Distrito>> GetAllDistrictsAsync()
        {
            return await dashboardRepository.GetAllDistrictsAsync();
        }

        public async Task<List<Incidente>> GetAllIncidentsAsync()
        {
            return await dashboardRepository.GetAllIncidentsAsync();
        }

        public async Task<List<Paciente>> GetAllPacientes()
        {
            return await dashboardRepository.GetAllPacientes();
        }

        public async Task<IEnumerable<Incidente>> GetByConditionAsync(Expression<Func<Incidente, bool>> expression)
        {
            return await dashboardRepository.GetByConditionAsync(expression);
        }

        public async Task<Incidente> GetByKeyAsync(string key)
        {
            return await dashboardRepository.GetByKeyAsync(key);
        }

        public async Task<int> GetIncidentsCounterAsync(string modality, string filter)
        {
            return await dashboardRepository.GetIncidentsCounterAsync(modality, filter);
        }

        public async Task<Incidente> InsertAsync(Incidente model)
        {
            return await dashboardRepository.InsertAsync(model);
        }

        public async Task UpdateAsync(Incidente model)
        {
            await dashboardRepository.UpdateAsync(model);
        }
    }
}
