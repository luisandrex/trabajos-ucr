using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Infrastructure.DataProviders;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Appointments
{
    public class SpecializesRepository : GenericRepository<SeEspecializa, string>, ISpecializesRepository
    {
        public SpecializesRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {

        }

        public async Task<IEnumerable<Persona>> GetDoctorsWithSpecialty(string specialty_name) {

            await using var connection = new SqlConnection(_db.DbConnection.ConnectionString);

            var result = await connection.ExecuteQueryAsync<Persona>
            (@"SELECT * FROM Persona p join Médico m on p.Cédula = m.Cédula join SeEspecializa s on m.Cédula = s.CedulaMedico 
            join EspecialidadMedica e on e.Nombre = s.NombreEspecialidad WHERE s.NombreEspecialidad = @specialty", new
            {
                specialty = specialty_name
            });
            return result; 
        }

    }
}
