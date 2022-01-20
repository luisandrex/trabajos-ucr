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
    public class SecurePacienteRepository : IPacienteRepository
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly PacienteRepository pacienteRepository;

        public SecurePacienteRepository(ISqlDataProvider dataProvider,
            IPrimeSecurityService _primeSecurityService)
        {
            primeSecurityService = _primeSecurityService;
            pacienteRepository = new PacienteRepository(dataProvider);
        }

        public async Task<Paciente> InsertPatientOnlyAsync(Paciente entity)
        {
            return await pacienteRepository.InsertPatientOnlyAsync(entity);
        }

        public async Task<Paciente> GetByKeyAsync(string key)
        {
            return await pacienteRepository.GetByKeyAsync(key);
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            return await pacienteRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Paciente>> GetByConditionAsync(Expression<Func<Paciente, bool>> expression)
        {
            return await pacienteRepository.GetByConditionAsync(expression);
        }

        public async Task<Paciente> InsertAsync(Paciente model)
        {
            return await pacienteRepository.InsertAsync(model);
        }

        public async Task DeleteAsync(string key)
        {
            await pacienteRepository.DeleteAsync(key);
        }

        public async Task UpdateAsync(Paciente model)
        {
            await pacienteRepository.UpdateAsync(model);
        }
    }
}
