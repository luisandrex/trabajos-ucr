using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.MedicalRecords;
using PRIME_UCR.Components.Incidents.IncidentDetails.Constants;
using PRIME_UCR.Components.Incidents.IncidentDetails.Tabs;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;


namespace PRIME_UCR.Components.MedicalAppointments
{
    public partial class MAActiveTab
    {
        [Parameter]
        public MADetailsTab Active { get; set; }


        [Parameter] public string med_appointment_id { get; set; }


        [Parameter] public string id { get; set; }


        [Parameter] public CitaMedica MedAppointment { get; set; }

        [Parameter] public int appointment_id { get; set; }

        [Parameter] public Cita Appointment { get; set; }

        public List<PoseeReceta> medicalprescrip { get; set; }

        public bool drug_selector_active { get; set; } = true;

        public bool prescription_description_not_done { get; set; } = false;



        protected override void OnInitialized()
        {
            medicalprescrip = new List<PoseeReceta>();
            base.OnInitialized();
        }


        protected override async Task OnInitializedAsync()
        {
            await get_prescriptions();
        }


        private async Task updateChanges(bool action)
        {

            prescription_description_not_done = false;
            drug_selector_active = true;
            await get_prescriptions();
        }

        private async Task get_prescriptions()
        {
            IEnumerable<PoseeReceta> records = await appointment_service.GetPrescriptionsByAppointmentId(Convert.ToInt32(id));
            medicalprescrip = records.ToList();
            StateHasChanged();
        }

        private async Task updatelist(bool f)
        {
            drug_selector_active = false;
            await get_prescriptions();
            StateHasChanged();
        }



    }
}
