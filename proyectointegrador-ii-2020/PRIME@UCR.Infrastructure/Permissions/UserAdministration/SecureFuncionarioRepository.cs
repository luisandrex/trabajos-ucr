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
    public class SecureFuncionarioRepository : IFuncionarioRepository
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly FuncionarioRepository funcionarioRepository;

        public SecureFuncionarioRepository(ISqlDataProvider dataProvider,
            IPrimeSecurityService _primeSecurityService)
        {
            primeSecurityService = _primeSecurityService;
            funcionarioRepository = new FuncionarioRepository(dataProvider);
        }

        public async Task<List<Funcionario>> GetAllAsync()
        {
            await primeSecurityService.CheckIfIsAuthorizedAsync(new[] { AuthorizationPermissions.CanAssignIncidents });
            return await funcionarioRepository.GetAllAsync();
        }


    }
}

