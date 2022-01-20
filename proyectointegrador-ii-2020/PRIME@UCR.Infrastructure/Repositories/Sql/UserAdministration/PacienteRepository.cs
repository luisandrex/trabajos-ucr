using PRIME_UCR.Application.Repositories.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepoDb;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Application.DTOs.UserAdministration;
using PRIME_UCR.Application.Exceptions.UserAdministration;
using System.Reflection;
using PRIME_UCR.Infrastructure.Permissions.UserAdministration;
using System.ComponentModel.DataAnnotations;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration
{
    internal class PacienteRepository : IPacienteRepository
    {
        private readonly ISqlDataProvider _db;

        public PacienteRepository(ISqlDataProvider dataProvider)
        {
            _db = dataProvider;
        }

        public async Task<Paciente> InsertPatientOnlyAsync(Paciente entity)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                await connection.ExecuteNonQueryAsync(
                    "dbo.InsertarPacienteSolo",
                    new { Cedula = entity.Cédula },
                    CommandType.StoredProcedure);

                return entity;
            }
        }

        public async Task<Paciente> GetByKeyAsync(string key)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var result = await connection.ExecuteQueryAsync<Paciente>(@"
                    select Paciente.Cédula, Nombre, PrimerApellido, SegundoApellido, Sexo, FechaNacimiento from Persona
                    join Paciente on Persona.Cédula = Paciente.Cédula
                    where Paciente.Cédula = @Ced
                ", new { Ced = key });
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var result = await connection.ExecuteQueryAsync<Paciente>(@"
                    select Paciente.Cédula, Nombre, PrimerApellido, SegundoApellido, Sexo, FechaNacimiento from Persona
                    join Paciente on Persona.Cédula = Paciente.Cédula
                ");
                return result;
            }
        }

        public async Task<IEnumerable<Paciente>> GetByConditionAsync(Expression<Func<Paciente, bool>> expression)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.QueryAsync(expression);
            }
        }

        public async Task<Paciente> InsertAsync(Paciente model)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var result = (await connection.QueryAsync<Persona>(model.Cédula)).FirstOrDefault();
                if (result == null)
                {
                    await connection.InsertAsync<Persona>(model);
                }
                
                await connection.InsertAsync(nameof(Paciente), new {model.Cédula});
                return model;
            }
        }

        public async Task DeleteAsync(string key)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                await connection.DeleteAsync(nameof(Paciente), key as object);
            }
        }

        public async Task UpdateAsync(Paciente model)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                await connection.UpdateAsync(nameof(Paciente), new {model.Cédula});
            }
        }
    }
}
