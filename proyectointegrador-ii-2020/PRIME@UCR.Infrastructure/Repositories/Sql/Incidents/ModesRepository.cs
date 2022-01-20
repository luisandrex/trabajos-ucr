using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Infrastructure.DataProviders;
using RepoDb;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Incidents
{
    public class ModesRepository : IModesRepository
    {
        private readonly ISqlDataProvider _db;

        public ModesRepository(ISqlDataProvider db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Modalidad>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.QueryAllAsync<Modalidad>();
            }
        }
    }
}