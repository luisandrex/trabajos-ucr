using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.Dashboard
{
    public class DashboardDataModel
    {
        public DashboardDataModel()
        {
            incidentsData = new List<Incidente>();
            medicalCenters = new List<CentroMedico>();
            modalities = new List<Modalidad>();
            states = new List<Estado>();
            districts = new List<Distrito>();
            filteredIncidentsData = new List<Incidente>();
            patients = new List<Paciente>();
            isReadyToShowFilters = false;
            isReadyToShowGraphs = false;
        }

        public List<Incidente> incidentsData { get; set; }

        public List<CentroMedico> medicalCenters { get; set; }

        public List<Modalidad> modalities { get; set; }
        
        public List<Estado> states { get; set; }
        
        public List<Pais> countries { get; set; }
        
        public List<Distrito> districts { get; set; }

        public List<Incidente> filteredIncidentsData { get; set; }

        public List<Paciente> patients { get; set; }

        public bool isReadyToShowFilters { get; set; }

        public bool isReadyToShowGraphs { get; set; }

        public string userEmail { get; set;}

        //Appointments
        public List<CitaMedica> appointmentsData { get; set; }
        public List<CitaMedica> filteredAppointmentsData { get; set; }
    }
}
