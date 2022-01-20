using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using RepoDb;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Incidents
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly ISqlDataProvider _db;

        public AssignmentRepository(ISqlDataProvider db)
        {
            _db = db;
        }

        public async Task<IEnumerable<EspecialistaTécnicoMédico>> GetAssignmentsByIncidentIdAsync(string code)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var result = await connection.ExecuteQueryAsync<EspecialistaTécnicoMédico>(@"
                    select P.Cédula, Nombre, PrimerApellido, SegundoApellido, Sexo, FechaNacimiento
                    from AsignadoA
                    join EspecialistaTécnicoMédico ETM on AsignadoA.CedulaEspecialista = ETM.Cédula
                    join Persona P on ETM.Cédula = P.Cédula
                    where AsignadoA.Codigo = @Code
                ", new {Code = code});

                return result;
            }
        }

        public async Task AssignToIncident(string code, IEnumerable<EspecialistaTécnicoMédico> specialistIds)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                var items = specialistIds.Select(etm => new AsignadoA
                {
                    CedulaEspecialista = etm.Cédula,
                    Codigo = code
                });
                
                await connection.InsertAllAsync(items);
            }
        }

        public async Task ClearTeamMembers(string code)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                await connection.ExecuteNonQueryAsync(@"
                    delete from AsignadoA
                    where Codigo = @Code
                ", new {Code = code});
            }
        }
    }
}