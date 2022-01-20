using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.Appointments
{
    public interface IHavePrescriptionRepository : IGenericRepository<PoseeReceta, int>
    {
        Task<IEnumerable<PoseeReceta>> GetPrescriptionByAppointmentId(int id);

        Task<PoseeReceta> GetPrescriptionByDrugId(int drug_id, int appointmentId);

        Task<PoseeReceta> UpdatePescription(int drug_id, int appointment_id, string dosis); 

    }
}
