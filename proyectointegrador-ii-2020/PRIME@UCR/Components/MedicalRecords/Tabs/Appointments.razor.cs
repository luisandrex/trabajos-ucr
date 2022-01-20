using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Application.Services.Incidents; 
using PRIME_UCR.Application.DTOs.MedicalRecords;
using PRIME_UCR.Domain.Models;
using BlazorTable;
using PRIME_UCR.Domain.Models.Appointments;
using MatBlazor;

namespace PRIME_UCR.Components.MedicalRecords.Tabs
{
    public partial class Appointments
    {
        [Parameter] public Expediente medical_record {get; set;}

        public List<Cita> appointments { get; set; }

        public List<Incidente> incidents { get; set; }

        public List<DateIncidentModel> dat_in_link { get; set; }

        //public List<bool> is_it_incident { get; set; }

        public ITable<Cita> AppointmentsModel { get; set;  }

        public ITable<DateIncidentModel> AppointmIncidentModel { get; set; }

        public bool are_there_appointments { get; set; } = false;

        public const string inci = "incidente";


        MatTheme AddButtonTheme = new MatTheme()
        {
            Primary = "white",
            Secondary = "#095290"
        };

        /*
        public string get_patient_name() {
            return medical_record.Paciente.NombreCompleto; 
        }

        public string get_record_id() {
            return medical_record.Id.ToString(); 
        }

        public string get_patient_identification() {
            return medical_record.Paciente.Cédula; 
        }
        */

        void Redirect()
        {
            //allow the program to navigate through different pages. 
            string path = "create-medical-appointment/" + medical_record.Id.ToString(); 
            NavManager.NavigateTo($"{path}");
        }


        protected override async Task OnParametersSetAsync()
        {

            if (medical_record != null && medical_record.Citas != null)
            {
                appointments = medical_record.Citas;
                are_there_appointments = true; 
            }
            else {
                if (medical_record == null)
                {
                    //no esta llegando nunca registro
                }
                else { 
                    //el registro no posee citas. 
                }
            }

            await getIncidents(); 

        }

        public string get_date_link(string recordId)
        {
            return $"/incidents/{recordId}";
        }


        
        public string get_appointment_link(int id) {
            if (true)
            {
                return $"/medical-appointment/{id}";
            }
            else {
                return $"/medical-appointment-actions/{id}";
            }
        }
        

        public async Task getIncidents() {
            dat_in_link = new List<DateIncidentModel>();

            for (int index = 0; index < appointments.Count; ++index) {

                Incidente incident = await incident_service.GetIncidentByDateCodeAsync(appointments[index].Id);

                if (incident != null)
                {
                    CentroMedico mc = new CentroMedico();
                    if (incident.IdDestino != null) {

                        mc = await medical_record_service.GetMedicalCenterByUbicationCenterIdAsync((int)incident.IdDestino); 
                    }

                    DateIncidentModel d_i = new DateIncidentModel()
                    {
                        date = appointments[index],
                        incident = incident,
                        medical_center = mc,
                        appointment_status = false
                    };

                    dat_in_link.Add(d_i);
                }
                else {

                    CitaMedica appointment = await appointment_service.GetMedicalAppointmentByAppointmentId(appointments[index].Id);

                    DateIncidentModel d_i = new DateIncidentModel()
                    {
                        date = appointments[index],
                        appointment = appointment
                    };


                    dat_in_link.Add(d_i); 
                }

            }
        }

    }
}
