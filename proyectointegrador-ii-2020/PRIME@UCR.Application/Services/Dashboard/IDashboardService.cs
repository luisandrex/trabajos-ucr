using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<List<Incidente>> GetAllIncidentsAsync();

        Task<List<Incidente>> GetFilteredIncidentsList(FilterModel Value);

        Task<List<Distrito>> GetAllDistrictsAsync();

        Task<int> GetIncidentCounterAsync(string modality, string filter);


        //Appointments
        Task<List<CitaMedica>> GetFilteredMedicalAppointmentsAsync(AppointmentFilterModel Value, List<CentroMedico> medicalCenters, List<Paciente> patients);

        Task<List<CitaMedica>> GetAllMedicalAppointmentsAsync();

        Task<List<Cita>> GetAllAppointments();

        Task<List<Paciente>> GetAllPacientes();
    }
}
