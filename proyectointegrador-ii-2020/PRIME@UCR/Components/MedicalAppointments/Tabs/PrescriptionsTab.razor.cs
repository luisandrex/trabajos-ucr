using Microsoft.AspNetCore.Components;
using PRIME_UCR.Components.MedicalRecords.Constants;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PRIME_UCR.Components.MedicalAppointments.Tabs
{
    public partial class PrescriptionsTab
    {

        [Parameter] public string id { get; set; }

        [Parameter] public Paciente Pacient { get; set; }
        public RecordSummary Summary;

        public List<PoseeReceta> medicalprescrip { get; set; }

        public bool drug_selector_active { get; set; } = true;

        public bool prescription_description_not_done { get; set; } = false;



        protected override void OnInitialized()
        {
            medicalprescrip = new List<PoseeReceta>();
            base.OnInitialized(); 
            Summary = new RecordSummary();
            Summary.LoadPatientValues(Pacient);
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
