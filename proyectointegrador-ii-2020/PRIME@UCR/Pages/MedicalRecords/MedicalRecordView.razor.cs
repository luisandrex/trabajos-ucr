using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using PRIME_UCR.Components.MedicalRecords;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using System.Runtime.CompilerServices;
using PRIME_UCR.Application.DTOs.MedicalRecords;
using System.Linq;
using PRIME_UCR.Application.Services.MedicalRecords;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Application.Implementations.MedicalRecords;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Components.MedicalRecords.Constants;

namespace PRIME_UCR.Pages.MedicalRecords
{
    public partial class MedicalRecordView
    {

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string state { get; set; }

        public bool new_record { get; set; } = false; 

        private readonly List<Tuple<DetailsTab, string>> _tabs = new List<Tuple<DetailsTab, string>>();

        const DetailsTab DefaultTab = DetailsTab.Info;

        private DetailsTab _activeTab = DefaultTab;

        protected bool exists = true;

        private RecordViewModel viewModel = new RecordViewModel();

        private List<Antecedentes> antecedentes;

        private List<Alergias> alergias;

        private List<PadecimientosCronicos> PadecimientosCronicos;

        private List<ListaAntecedentes> ListaAntecedentes;

        private List<ListaAlergia> ListaAlergias;

        private List<ListaPadecimiento> ListaPadecimiento;

        private Cita lastAppointment = new Cita();

        public RecordSummary Summary;


        Expediente medical_record_with_details { get; set; }

        private void FillTabStates()
        {
            _tabs.Clear();
            var tabValues = Enum.GetValues(typeof(DetailsTab)).Cast<DetailsTab>();
            foreach (var tab in tabValues)
            {
                switch (tab)
                {
                    case DetailsTab.Info:
                        _tabs.Add(new Tuple<DetailsTab, string>(DetailsTab.Info, ""));
                        break;
                    case DetailsTab.Appointments:
                        _tabs.Add(new Tuple<DetailsTab, string>(DetailsTab.Appointments, ""));
                        break;
                    case DetailsTab.MedicalBackgroundTab:
                        _tabs.Add(new Tuple<DetailsTab, string>(DetailsTab.MedicalBackgroundTab, ""));
                        break;
                }
            }
        }
        public string get_patient_name()
        {
            string name = "";
            name += viewModel.Nombre + " " + viewModel.PrimerApellido + " " + viewModel.SegundoApellido;
            return name;
        }
        protected override async Task OnInitializedAsync()
        {
            int identification = Int32.Parse(Id);
            viewModel = await MedicalRecordService.GetIncidentDetailsAsync(identification);
            Summary = new RecordSummary();
            Summary.LoadValues(viewModel);
            //Get all background item related to a record by its id
            antecedentes = (await MedicalBackgroundService.GetBackgroundByRecordId(identification)).ToList();
            //Get all alergies related to a record by its id
            alergias = (await AllergyService.GetAlergyByRecordId(identification)).ToList();
            //Get all Chronic Conditions related to arecord by its id
            PadecimientosCronicos = (await ChronicConditionService.GetChronicConditionByRecordId(identification)).ToList();
            //Get all available background items.
            ListaAntecedentes = (await MedicalBackgroundService.GetAll()).ToList();
            //Get all available alergies
            ListaAlergias = (await AllergyService.GetAll()).ToList();
            //Get all available Chronic Conditions
            ListaPadecimiento = (await ChronicConditionService.GetAll()).ToList();
            //Get all dates related to the medical record. 
            medical_record_with_details = await MedicalRecordService.GetMedicalRecordDetailsLinkedAsync(identification);
            //Get last appointment
            lastAppointment = await AppointmentService.GetLastAppointmentDateAsync(identification);

            if (viewModel == null)
                exists = false;
            else
                FillTabStates();

            if (state != null && state == "created") {
                new_record = true; 
            }

        }

    }
}
