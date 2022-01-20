using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Infrastructure.DataProviders;
using RepoDb;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Incidents
{
    public class IncidentStateRepository : IIncidentStateRepository
    {
        private readonly ISqlDataProvider _db;

        public IncidentStateRepository(ISqlDataProvider db)
        {
            _db = db;
        }

        public async Task AddState(EstadoIncidente incidentState)
        {
            if (incidentState == null)
                throw new ArgumentNullException("incidentState");
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                // specify composite key, use raw SQL to support this
                await connection.ExecuteNonQueryAsync(
                    @"
                        update EstadoIncidente
                        set Activo = 0
                        where CodigoIncidente = @Id
                    ", new
                    {
                        Id = incidentState.CodigoIncidente,
                    }
                );

                incidentState.Activo = true; // make sure it is inserted as active
                incidentState.FechaHora = DateTime.Now;
                await connection.InsertAsync(incidentState);
            }
        }

        public async Task<Estado> GetCurrentStateByIncidentId(string incidentCode)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var result =
                    (await connection.QueryAsync<EstadoIncidente>(
                        e => e.Activo == true &&
                             e.CodigoIncidente == incidentCode))
                    .FirstOrDefault();

                return
                    (await connection.QueryAsync<Estado>(e =>
                        e.Nombre == (result != null ? result.NombreEstado : null)))
                    .FirstOrDefault();
            }
        }

        public async Task<IEnumerable<EstadoIncidente>> GetIncidentStatesByIncidentId(string incidentCode)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.QueryAsync<EstadoIncidente>(e =>
                    e.CodigoIncidente == incidentCode
                );
            }
        }
    }
}