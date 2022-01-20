using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RepoDb;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Incidents
{
    public class TransportUnitRepository : RepoDbRepository<UnidadDeTransporte, string>, ITransportUnitRepository
    {
        public TransportUnitRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }

        public async Task<IEnumerable<UnidadDeTransporte>> GetAllTransporUnitsByMode(string mode)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.QueryAsync<UnidadDeTransporte>(t =>
                    t.Modalidad == mode
                );
            }
        }

        public async Task<UnidadDeTransporte> GetTransporUnitByIncidentIdAsync(string incidentId)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return
                    (await connection.ExecuteQueryAsync<UnidadDeTransporte>(
                        @"
                            select UDT.*
                            from Incidente
                            join Unidad_De_Transporte UDT on Incidente.MatriculaTrans = UDT.Matricula
                            where Incidente.Codigo = @Id
                        ", new
                        {
                            Id = incidentId
                        }
                    ))
                    .FirstOrDefault();
            }
        }
    }
}
