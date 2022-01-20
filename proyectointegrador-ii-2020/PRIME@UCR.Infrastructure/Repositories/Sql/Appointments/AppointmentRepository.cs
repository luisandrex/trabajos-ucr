using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories;
using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Infrastructure.DataProviders;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.Appointments
{
    public class AppointmentRepository : RepoDbRepository<Cita, int>, IAppointmentRepository
    {
        public AppointmentRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {
        }

        public async Task<Cita> getLatestAppointmentByRecordId(int id)
        {
            return await _db.Appointments
                            .OrderByDescending(x => x.FechaHoraEstimada)
                            .Where(x => x.IdExpediente == id && x.FechaHoraEstimada <= DateTime.Now)
                            .FirstOrDefaultAsync();
        }

        public async Task<Cita> GetAppointmentWithRecordNPatientByKeyAsync(int id) {
            return await _db.Appointments
                            .Include(p => p.Expediente)
                            .Include(p => p.Expediente.Paciente)
                            .FirstOrDefaultAsync(p => p.Id == id);
        
        }

    }
}