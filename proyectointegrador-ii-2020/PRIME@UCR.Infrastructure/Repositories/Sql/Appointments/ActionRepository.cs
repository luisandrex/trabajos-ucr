using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoDb;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Appointments
{
    public class ActionRepository : IActionRepository
    {
        protected readonly ISqlDataProvider _db;

        public ActionRepository(ISqlDataProvider dataProvider)
        {
            _db = dataProvider;
        }

        public async Task DeleteAsync(int citaId, string nombreAccion, int mcId)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                await connection.ExecuteNonQueryAsync(
                    @"
                        delete from Accion
                        where CitaId = @CitaId
                            and MultContId = @MultContId
                            and NombreAccion = @NombreAccion
                    ", new
                    {
                        CitaId = citaId,
                        MultContId = mcId,
                        NombreAccion = nombreAccion
                    }
                );
            }
        }

        public async Task<IEnumerable<MultimediaContent>> GetByAppointmentAction(int citaId, string nombreAccion)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                return await connection.ExecuteQueryAsync<MultimediaContent>(
                    @"
                        select MC.*
                        from Accion
                        join MultimediaContent MC on MC.Id = Accion.MultContId
                        where CitaId = @CitaId and NombreAccion = @NombreAccion
                    ",
                    new
                    {
                        CitaId = citaId,
                        NombreAccion = nombreAccion
                    }
                );
            }
        }

        public async Task<Accion> InsertAsync(Accion action)
        {
            using (var connection = new SqlConnection(_db.ConnectionString))
            {
                await connection.InsertAsync(action);
                return action;
            }
        }
    }
}
