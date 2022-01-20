using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Application.Services.Appointments
{
    public interface IAppointmentService
    {
        Task<IEnumerable<TipoAccion>> GetActionTypesAsync(bool isIncident = true);

        Task<IEnumerable<TipoAccion>> GetActionsTypesMedicalAppointmentAsync(bool isMedAppointment = true); 

        Task<Expediente> AssignMedicalRecordAsync(int appointmentId, Paciente patient);
        Task<Cita> GetLastAppointmentDateAsync(int id);

        Task<CitaMedica> GetMedicalAppointmentByAppointmentId(int id);

        Task<IEnumerable<PoseeReceta>> GetPrescriptionsByAppointmentId(int id);

        Task<IEnumerable<RecetaMedica>> GetDrugsAsync();

        Task<EstadoCitaMedica> GetStatusById(int id);

        Task<IEnumerable<RecetaMedica>> GetDrugsByConditionAsync(string drugname);

        Task<IEnumerable<RecetaMedica>> GetDrugsByFilterAsync(string filter); 

        Task<PoseeReceta> GetDrugByConditionAsync(int drug_id, int appointmentId);

        Task UpdateAppointmentStatus(int id);

        Task<PoseeReceta> InsertPrescription(int idMedicalPrescription, int idMedicalAppointment);

        Task<PoseeReceta> UpdatePrescriptionDosis(int idMedicalPrescription, int idMedicalAppointment, string dosis);

        Task UpdateAsync(PoseeReceta prescription);

        Task<CitaMedica> GetMedicalAppointmentByKeyAsync(int id);

        Task<CitaMedica> GetMedicalAppointmentWithAppointmentByKeyAsync(int id);

        Task<Cita> GetAppointmentByKeyAsync(int id);

        Task<CitaMedica> GetMedicalAppointmentWithAppointmentByIdAsync(int id);

        Task<CentroMedico> GetMedCenterByKeyAsync(int id);

        Task<MetricasCitaMedica> GetMetricsMedAppointmentByAppId(int id);

        Task InsertMetrics(MetricasCitaMedica metrics);

        Task UpdateMetrics(MetricasCitaMedica metrics);

        Task<CitaMedica> InsertMedicalAppointmentAsync(CitaMedica model);

        Task<Cita> InsertAppointmentAsync(Cita model);

        Task<IEnumerable<EspecialidadMedica>> GetMedicalSpecialtiesAsync();

        Task<IEnumerable<Persona>> GetDoctorsBySpecialtyNameAsync(string specialty_name);

        Task InsertAppointmentReference(ReferenciaCita reference);


        Task<IEnumerable<ReferenciaCita>> GetReferencesByAppId(int id);
        // Task<EstadoCitaMedica> GetMedAppointmentStatusAsync(int id); 
    }
}
