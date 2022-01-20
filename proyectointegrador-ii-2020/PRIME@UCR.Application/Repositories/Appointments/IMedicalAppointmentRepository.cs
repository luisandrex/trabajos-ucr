using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.Appointments
{
    public interface IMedicalAppointmentRepository : IGenericRepository<CitaMedica, int>
    {
        Task<CitaMedica> GetByAppointmentId(int id);

        Task<CitaMedica> GetMedicalAppointmentWithAppointmentByKeyAsync(int id);

        Task<CitaMedica> GetMedicalAppointmentByWithAppointmentIdAsync(int id);

        Task<IEnumerable<CitaMedica>> GetAllMedicalAppointmentsAsync();

    }
}
