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
    public class HavePrescriptionRepository : GenericRepository<PoseeReceta, int>, IHavePrescriptionRepository
    {
        public HavePrescriptionRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {

        }

        public async Task<PoseeReceta> GetPrescriptionByDrugId(int id, int appointmentId) {
            return await _db.HavePrescription
                          .Where(p => p.IdRecetaMedica == id && p.IdCitaMedica == appointmentId)
                          .FirstOrDefaultAsync(); 
        }


        public async Task<PoseeReceta> UpdatePescription(int drug_id, int appointment_id, string dosis) {

            await using var connection = new SqlConnection(_db.DbConnection.ConnectionString);

            var result = await connection.ExecuteQueryAsync<PoseeReceta>
            (@"UPDATE PoseeReceta SET Dosis = @dosis_ WHERE IdRecetaMedica = @drugId and IdCitaMedica = @appId", new
            {
                dosis_ = dosis,
                drugId = drug_id,
                appId = appointment_id
            });

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<PoseeReceta>> GetPrescriptionByAppointmentId(int id) {
            return await _db.HavePrescription
                .Include(p => p.RecetaMedica)
                .Where(p => p.IdCitaMedica == id)
                .ToListAsync();
        }

    }
}
