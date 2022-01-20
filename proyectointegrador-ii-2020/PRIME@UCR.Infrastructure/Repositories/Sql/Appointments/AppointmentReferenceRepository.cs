using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Infrastructure.DataProviders;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Appointments
{
    public class AppointmentReferenceRepository : GenericRepository<ReferenciaCita, int>, IAppointmentReferenceRepository
    {
        public AppointmentReferenceRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {

        }


        public async Task<IEnumerable<ReferenciaCita>> GetReferencesByAppId(int id) {
            if (true)
            {
                await using var connection = new SqlConnection(_db.DbConnection.ConnectionString);
                var result = await connection.ExecuteQueryAsync<ReferenciaCita>
                (@"SELECT * FROM ReferenciaCita WHERE IdCita1 = @id1", new
                {
                    id1 = id,
                });

                return result;
            }
            else {
                return await _db.AppointmentReference
                                .Include(p => p.Cita1)
                                .Include(p => p.Cita2)
                                .Where(p => p.IdCita1 == id)
                                .ToListAsync(); 
            
            }
        }

        public async Task InsertReference(ReferenciaCita reference) {

            await using var connection = new SqlConnection(_db.DbConnection.ConnectionString);
            var result = await connection.ExecuteQueryAsync<ReferenciaCita>
            (@"INSERT INTO ReferenciaCita(IdCita1, IdCita2, Especialidad) VALUES(@id1, @id2, @specialty)", new
            {
                id1 =reference.IdCita1,
                id2 = reference.IdCita2,
                specialty = reference.Especialidad
            });

        }

    }
}
