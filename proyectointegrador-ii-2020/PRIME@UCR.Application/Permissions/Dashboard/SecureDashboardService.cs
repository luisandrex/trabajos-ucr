using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.Implementations.Dashboard;
using PRIME_UCR.Application.Repositories.Appointments;
using PRIME_UCR.Application.Repositories.Dashboard;
using PRIME_UCR.Application.Repositories.Incidents;
using PRIME_UCR.Application.Services.Dashboard;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Attributes;
using PRIME_UCR.Domain.Constants;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Permissions.Dashboard
{
    public class SecureDashboardService : IDashboardService
    {
        private readonly IPrimeSecurityService primeSecurityService;

        private readonly DashboardService dashboardService;

        public SecureDashboardService(IDashboardRepository dashboardRep,
            IIncidentRepository _incidentRepository,
            IDistrictRepository _districtRepository,
            ICountryRepository _countryRepository,
            IMedicalCenterRepository _medicalCenterRepository,
            IPrimeSecurityService _primeSecurityService,
            IAppointmentRepository _appointmentRepository,
            IMedicalAppointmentRepository _medicalAppointmentRepository)
        {
            primeSecurityService = _primeSecurityService;
            dashboardService = new DashboardService(dashboardRep, _incidentRepository, _districtRepository, _countryRepository, _medicalCenterRepository, _appointmentRepository, _medicalAppointmentRepository);
        }

        public async Task<int> GetIncidentCounterAsync(string modality, string filter)
        {
            return await dashboardService.GetIncidentCounterAsync(modality, filter);
        }
        
        public async Task<List<Distrito>> GetAllDistrictsAsync()
        {
            return await dashboardService.GetAllDistrictsAsync();
        }

        public async Task<List<Incidente>> GetAllIncidentsAsync()
        {
            return await dashboardService.GetAllIncidentsAsync();
        }

        public async Task<List<Incidente>> GetFilteredIncidentsList(FilterModel Value)
        {
            return await dashboardService.GetFilteredIncidentsList(Value);
        }

        public async Task<List<CitaMedica>> GetFilteredMedicalAppointmentsAsync(AppointmentFilterModel Value, List<CentroMedico> medicalCenters, List<Paciente> patients)
        {
            return await dashboardService.GetFilteredMedicalAppointmentsAsync(Value, medicalCenters, patients);
        }

        public async Task<List<CitaMedica>> GetAllMedicalAppointmentsAsync()
        {
            return await dashboardService.GetAllMedicalAppointmentsAsync();
        }

        public async Task<List<Cita>> GetAllAppointments()
        {
            return await dashboardService.GetAllAppointments();
        }

        public async Task<List<Paciente>> GetAllPacientes()
        {
            return await dashboardService.GetAllPacientes();
        }
    }
}
