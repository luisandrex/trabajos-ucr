using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Infrastructure.DataProviders;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Domain.Models.UserAdministration;
using RepoDb;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Incidents
{
    public class MedicalCenterRepository : RepoDbRepository<CentroMedico, int>, IMedicalCenterRepository
    {
        public MedicalCenterRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }

        public override async Task<IEnumerable<CentroMedico>> GetAllAsync() {
            return await _db.MedicalCenters.ToListAsync();
        }

        public async Task<IEnumerable<Médico>> GetDoctorsByMedicalCenterId(int id)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.ExecuteQueryAsync<Médico>(
                    @"
                    select P.*
                    from Trabaja_En
                    join Médico M on Trabaja_En.CédulaMédico = M.Cédula
                    join Funcionario F on M.Cédula = F.Cédula
                    join Persona P on F.Cédula = P.Cédula
                    where Trabaja_En.CentroMedicoId = @Id",
                    new {Id = id}
                );
            }
        }
    }
}