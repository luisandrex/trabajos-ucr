using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
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
    internal class CoordinadorTécnicoMédicoRepository : ICoordinadorTécnicoMédicoRepository
    {
        private readonly ISqlDataProvider _db;

        public CoordinadorTécnicoMédicoRepository(ISqlDataProvider dataProvider)
        {
            _db = dataProvider;
        }

        public async Task<CoordinadorTécnicoMédico> GetByKeyAsync(string key)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var result = await connection.ExecuteQueryAsync<CoordinadorTécnicoMédico>(@"
                    select Persona.Cédula, Persona.Nombre, Persona.PrimerApellido, Persona.SegundoApellido, Persona.Sexo, Persona.FechaNacimiento
                    from Persona
                    join Funcionario F on Persona.Cédula = F.Cédula
                    join CoordinadorTécnicoMédico CTM on F.Cédula = CTM.Cédula
                    where CTM.Cédula = @Ced
                ", new { Ced = key });
                
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<CoordinadorTécnicoMédico>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var result = await connection.ExecuteQueryAsync<CoordinadorTécnicoMédico>(@"
                    select Persona.Cédula, Persona.Nombre, Persona.PrimerApellido, Persona.SegundoApellido, Persona.Sexo, Persona.FechaNacimiento
                    from Persona
                    join Funcionario F on Persona.Cédula = F.Cédula
                    join CoordinadorTécnicoMédico CTM on F.Cédula = CTM.Cédula
                ");
                
                return result;
            }
        }

        public Task<IEnumerable<CoordinadorTécnicoMédico>> GetByConditionAsync(Expression<Func<CoordinadorTécnicoMédico, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<CoordinadorTécnicoMédico> InsertAsync(CoordinadorTécnicoMédico model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CoordinadorTécnicoMédico model)
        {
            throw new NotImplementedException();
        }
    }
}
