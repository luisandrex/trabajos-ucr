using Microsoft.AspNetCore.Components;
using PRIME_UCR.Components.MedicalAppointments;
using PRIME_UCR.Components.MedicalRecords.Constants;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Pages.Appointments
{
    public partial class MedicalAppointmentView
    {
        [Parameter] public string id { get; set; }

        //[Parameter] public DateIncidentModel record { get; set; }

        private readonly List<Tuple<MADetailsTab, string>> _tabs = new List<Tuple<MADetailsTab, string>>();

        const MADetailsTab DefaultTab = MADetailsTab.General;

        private MADetailsTab _activeTab = DefaultTab;

        protected bool exists = true;

        public CitaMedica appointment { get; set; }

        public RecordSummary Summary;

        public Cita appoint { get; set; }


        private void FillTabStates()
        {
            _tabs.Clear();
            var tabValues = Enum.GetValues(typeof(MADetailsTab)).Cast<MADetailsTab>();
            foreach (var tab in tabValues)
            {
                switch (tab)
                {
                    case MADetailsTab.Recetas:
                        _tabs.Add(new Tuple<MADetailsTab, string>(MADetailsTab.Recetas, ""));
                        break;
                    case MADetailsTab.Multimedia:
                        _tabs.Add(new Tuple<MADetailsTab, string>(MADetailsTab.Multimedia, ""));
                        break;
                    case MADetailsTab.General:
                        _tabs.Add(new Tuple<MADetailsTab, string>(MADetailsTab.General, ""));
                        break;
                    case MADetailsTab.Metricas:
                        _tabs.Add(new Tuple<MADetailsTab, string>(MADetailsTab.Metricas, ""));
                        break;
                    case MADetailsTab.Referencias:
                        _tabs.Add(new Tuple<MADetailsTab, string>(MADetailsTab.Referencias, ""));
                        break;
                }
            }
        }


        
        protected override async Task OnInitializedAsync()
        {
            //appointment = await appointment_service.GetMedicalAppointmentByKeyAsync(Convert.ToInt32(id));

            appointment = await appointment_service.GetMedicalAppointmentWithAppointmentByIdAsync(Convert.ToInt32(id)); 

            appoint = await appointment_service.GetAppointmentByKeyAsync(Convert.ToInt32(appointment.CitaId)); 

            //appointment = await appointment_service.GetMedicalAppointmentWithAppointmentByKeyAsync(Convert.ToInt32(id)); 

            Summary = new RecordSummary();
            Summary.LoadPatientValues(appoint.Expediente.Paciente);

            if (appointment == null)
                exists = false;
            else
                FillTabStates();

        }


    }
}
