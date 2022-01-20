using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.MedicalRecords;
using PRIME_UCR.Application.Implementations.MedicalRecords;
using PRIME_UCR.Components.Incidents.IncidentDetails.Constants;
using PRIME_UCR.Components.Incidents.IncidentDetails.Tabs;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Components.MedicalRecords
{
    public partial class ActiveTab
    {
        [Parameter]
        public DetailsTab Active { get; set; }

        [Parameter]
        public RecordViewModel Expediente { get; set; }

        [Parameter]
        public Expediente MedicalRecord { get; set;  }

        [Parameter] 
        public List<ListaAntecedentes> ListaAntecedentes { get; set; }

        [Parameter] 
        public List<Antecedentes> Antecedentes { get; set; }

        [Parameter] 
        public List<Alergias> Alergias { get; set; }

        [Parameter] 
        public List<ListaAlergia> ListaAlergia { get; set; }
        [Parameter]
        public List<PadecimientosCronicos> PadecimientosCronicos { get; set; }

        [Parameter]
        public List<ListaPadecimiento> ListaPadecimiento { get; set; }
        [Parameter]
        public Cita UltimaCita { get; set; }

        protected override async Task OnInitializedAsync() {
            Antecedentes = (await MedicalBackgroundService.GetBackgroundByRecordId(Expediente.IdExpediente)).ToList();
            PadecimientosCronicos = (await ChronicConditionService.GetChronicConditionByRecordId(Expediente.IdExpediente)).ToList();
            Alergias = (await AllergyService.GetAlergyByRecordId(Expediente.IdExpediente)).ToList();
        }
        public string get_patient_name()
        {
            return MedicalRecord.Paciente.NombreCompleto;
        }

        public string get_patient_identification()
        {
            return MedicalRecord.Paciente.Cédula;
        }
    }

}