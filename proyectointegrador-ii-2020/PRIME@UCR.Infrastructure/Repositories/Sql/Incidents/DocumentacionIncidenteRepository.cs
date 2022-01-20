using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Infrastructure.DataProviders;
using RepoDb;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Incidents
{
    public class DocumentacionIncidenteRepository : RepoDbRepository<DocumentacionIncidente, int>, IDocumentacionIncidenteRepository
    {
        public DocumentacionIncidenteRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }

        public async Task<IEnumerable<DocumentacionIncidente>> GetAllDocumentationByIncidentCode(string incidentCode)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.QueryAsync<DocumentacionIncidente>(d =>
                    d.CodigoIncidente == incidentCode
                );
            }
        }
    }
}
