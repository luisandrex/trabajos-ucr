using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Infrastructure.DataProviders;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Appointments
{
    public class DrugRepository : GenericRepository<RecetaMedica, int>, IDrugRepository
    {
        public DrugRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {

        }

        public async Task<IEnumerable<RecetaMedica>> GetDrugsByFilterAsync(string filter) {
            await using var connection = new SqlConnection(_db.DbConnection.ConnectionString);

            var result = await connection.ExecuteQueryAsync<RecetaMedica>
               (@"SELECT * FROM RecetaMedica WHERE NombreReceta like @drug_name", new
               {
                    drug_name = filter
               });
            return result; 
        }
    }
}
