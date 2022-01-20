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
using PRIME_UCR.Application.Exceptions.UserAdministration;
using PRIME_UCR.Application.DTOs.UserAdministration;
using System.Reflection;
using PRIME_UCR.Infrastructure.Permissions.UserAdministration;
using System.ComponentModel.DataAnnotations;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.UserAdministration
{
    internal class DoctorRepository : IDoctorRepository
    {
        private readonly ISqlDataProvider _db;

        public DoctorRepository(ISqlDataProvider dataProvider)
        {
            _db = dataProvider;
        }

        public async Task<Médico> GetByKeyAsync(string key)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var result = await connection.ExecuteQueryAsync<Médico>(@"
                    select Persona.Cédula, Nombre, PrimerApellido, SegundoApellido, Sexo, FechaNacimiento from Persona
                    join Médico M on Persona.Cédula = M.Cédula
                    where M.Cédula = @Ced
                ", new { Ced = key});
                
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Médico>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.ExecuteQueryAsync<Médico>($@"
                    select Persona.Cédula, Nombre, PrimerApellido, SegundoApellido, Sexo, FechaNacimiento from Persona
                    join Médico M on Persona.Cédula = M.Cédula
                ");
            }
        }

        public async Task<IEnumerable<Médico>> GetByConditionAsync(Expression<Func<Médico, bool>> expression)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.QueryAsync(expression);
            }
        }

        public async Task<Médico> InsertAsync(Médico model)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var result = (await connection.QueryAsync<Persona>(model.Cédula)).FirstOrDefault();
                if (result == null)
                {
                    await connection.InsertAsync<Persona>(model);
                }
                
                await connection.InsertAsync(nameof(Médico), new {model.Cédula});
                return model;
            }
        }

        public async Task DeleteAsync(string key)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                await connection.DeleteAsync(nameof(Médico), key as object);
            }
        }

        public async Task UpdateAsync(Médico model)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                await connection.UpdateAsync(nameof(Médico), new {model.Cédula});
            }
        }
    }
}
