using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Infrastructure.DataProviders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Infrastructure.Repositories.Sql.MedicalRecords
{
    public class MedicalAppointmentRepository : GenericRepository<CitaMedica, int>, IMedicalAppointmentRepository
    {
        public MedicalAppointmentRepository(ISqlDataProvider dataProvider) : base(dataProvider)
        {

        }

        public async Task<CitaMedica> GetByAppointmentId(int id)
        {
            return await _db.MedicalAppointment
                            .Include(p => p.medical_center)
                            .FirstOrDefaultAsync(ap => ap.CitaId == id); 
        }

        public async Task<CitaMedica> GetMedicalAppointmentByWithAppointmentIdAsync(int id) {
            return await _db.MedicalAppointment
                            .Include(p => p.Cita)
                            .Include(p => p.medical_center)
                            .FirstOrDefaultAsync(p => p.Id == id);  
        }

        public async Task<CitaMedica> GetMedicalAppointmentWithAppointmentByKeyAsync(int id) {
            return await _db.MedicalAppointment
                            .Include(p => p.Cita)
                            .Include(p => p.Cita.Expediente)
                            .FirstOrDefaultAsync(p => p.Id == id); 
        
        }

        public async Task<IEnumerable<CitaMedica>> GetAllMedicalAppointmentsAsync()
        {
            return await _db.MedicalAppointment.Include(p => p.Cita)
                            .ThenInclude(c => c.Metricas)
                            .Include(c => c.Cita.Expediente).ToListAsync();
                                
        }


    }
}
