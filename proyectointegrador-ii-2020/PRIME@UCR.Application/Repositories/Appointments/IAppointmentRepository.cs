using System.Collections.Generic;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Appointments;

namespace PRIME_UCR.Application.Repositories.Appointments
{
    public interface IAppointmentRepository : IRepoDbRepository<Cita, int>
    {
        Task<Cita> getLatestAppointmentByRecordId(int id);

        Task<Cita> GetAppointmentWithRecordNPatientByKeyAsync(int id);
    }
}