using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.Repositories.Dashboard;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using PRIME_UCR.Infrastructure.Permissions.Dashboard;
using RepoDb;
using RepoDb.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Dashboard
{
    internal class DashboardRepository : IDashboardRepository
    {
        public readonly ISqlDataProvider _db;

        public DashboardRepository(ISqlDataProvider sqlDataProvider)
        {
            _db = sqlDataProvider;
        }

        public async Task<int> GetIncidentsCounterAsync(string modality, string filter)
        {
            var result = 0;
            await Task.Run(() =>
            {
                using (var cmd = _db.DbConnection.CreateCommand())
                {
                    while (cmd.Connection.State != System.Data.ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    if (cmd.Connection.State == System.Data.ConnectionState.Open)
                    {
                        cmd.CommandText =
                            $"EXECUTE dbo.GetIncidentsCounter @modality='{modality}' , @filter='{filter}'";

                        var dbResult = cmd.ExecuteScalar();
                        result = int.Parse(dbResult.ToString());

                    }

                }
            });

            return await Task.FromResult(result);
        }

        /**
         * Method used to get the list of all the incidents.
         * 
         * Return: List of incidents.
         */
        public async Task<List<Incidente>> GetAllIncidentsAsync()
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var sql =
                    // Incidente
                    @"
                        select *
                        from Incidente;
                    " +
                    // Cita
                    @"
                        select C.*
                        from Incidente I
                        join Cita C on C.Id = I.CodigoCita;
                    " +
                    // Destino - CentroUbicacion
                    @"
                        select CU.*
                        from Incidente
                        left join Ubicacion U on Incidente.IdDestino = U.Id
                        left join Centro_Ubicacion CU on U.Id = CU.Id
                    " +
                    // Estado
                    @"
                        select EI.*
                        from Incidente
                        join EstadoIncidente EI on Incidente.Codigo = EI.CodigoIncidente
                        where EI.Activo = 1;
                    ";

                using (var result = await connection.ExecuteQueryMultipleAsync(sql))
                {
                    var incidents = await result.ExtractAsync<Incidente>();
                    var appointments = await result.ExtractAsync<Cita>();
                    var destinations = await result.ExtractAsync<CentroUbicacion>();
                    var medicalCenters = await connection.QueryAllAsync<CentroMedico>();
                    var states = await result.ExtractAsync<EstadoIncidente>();
                    var origins = _db.Incidents
                            .AsNoTracking()
                            .Include(i => i.Origen)
                            .Select(i => i.Origen);

                    foreach(var incident in incidents)
                    {
                        incident.Cita = appointments.FirstOrDefault(a => a.Id == incident.CodigoCita);
                        incident.Destino = destinations.FirstOrDefault(d => d.Id == incident.IdDestino);
                        incident.Origen = origins.FirstOrDefault(d => d.Id == incident.IdOrigen);
                        incident.EstadoIncidentes.AddRange(states.Where(s => s.CodigoIncidente == incident.Codigo));
                    }
                    return incidents.ToList();
                }
            }

        }

        /**
         * Method used to get the list of all the incidents join with origin information.
         * 
         * Return: List of incidents.
         */
        public async Task<List<Distrito>> GetAllDistrictsAsync()
        {
            var districts = new List<Distrito>();
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var returnedValue = await connection.ExecuteQueryMultipleAsync(@"
                    SELECT *
                    FROM Provincia;

                    SELECT *
                    FROM Canton;

                    SELECT *
                    FROM Distrito;
                ");

                var provinces = returnedValue.Extract<Provincia>().AsList();
                var cantons = returnedValue.Extract<Canton>().AsList();
                districts = returnedValue.Extract<Distrito>().AsList();

                foreach(var district in districts)
                {
                    district.Canton = cantons.FirstOrDefault(c => c.Id == district.IdCanton);
                    district.Canton.Provincia = provinces.FirstOrDefault(p => p.Nombre == district.Canton.NombreProvincia);
                }
            }
            return districts;
        }

        public Task<Incidente> GetByKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Incidente>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Incidente>> GetByConditionAsync(Expression<Func<Incidente, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Incidente> InsertAsync(Incidente model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Incidente model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Paciente>> GetAllPacientes()
        {
            using (var connection = new SqlConnection(_db.DbConnection.ConnectionString))
            {
                var result = await connection.ExecuteQueryAsync<Paciente>(@"
                    select Paciente.Cédula, Nombre, PrimerApellido, SegundoApellido, Sexo, FechaNacimiento from Persona
                    join Paciente on Persona.Cédula = Paciente.Cédula
                ");
                return result.ToList();
            }
        }
    }
}
