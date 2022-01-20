using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RepoDb;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Exceptions.UserAdministration;
using System.Reflection;
using PRIME_UCR.Infrastructure.Permissions.UserAdministration;
using System.ComponentModel.DataAnnotations;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration
{
    public partial class EspecialistaTécnicoMédicoRepository : IEspecialistaTécnicoMédicoRepository
    {
        private readonly ISqlDataProvider _db;

        public EspecialistaTécnicoMédicoRepository(ISqlDataProvider dataProvider)
        {
            _db = dataProvider;
        }

        public Task<EspecialistaTécnicoMédico> GetByKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EspecialistaTécnicoMédico>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var result = await connection.ExecuteQueryAsync<EspecialistaTécnicoMédico>(@"
                    select Persona.Cédula, Persona.Nombre, Persona.PrimerApellido, Persona.SegundoApellido, Persona.Sexo, Persona.FechaNacimiento
                    from Persona
                    join Funcionario F on Persona.Cédula = F.Cédula
                    join EspecialistaTécnicoMédico ETM on F.Cédula = ETM.Cédula
                ");
                return result;
            }
        }

        public Task<IEnumerable<EspecialistaTécnicoMédico>> GetByConditionAsync(Expression<Func<EspecialistaTécnicoMédico, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<EspecialistaTécnicoMédico> InsertAsync(EspecialistaTécnicoMédico model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(EspecialistaTécnicoMédico model)
        {
            throw new NotImplementedException();
        }
    }
}
