using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Components.Incidents.IncidentDetails.Tabs;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Components.Incidents.IncidentDetails
{
    public partial class ActiveTab
    {
        [Parameter] public DetailsTab Active { get; set; }    
        [Parameter] public IncidentDetailsModel Incident { get; set; }    
        [Parameter] public string StatusMessage { get; set; }
        [Parameter] public string StatusClass { get; set; }
        [Parameter] public EventCallback<IncidentDetailsModel> OnSave { get; set; }

        // Services needed to keep in track last changes in incidents
        [Inject] public IUserService UserService { get; set; }
        [Inject] public IAssignmentService AssignmentService { get; set; }
        [Inject] public IIncidentService IncidentService { get; set; }
        [Inject] public IPersonService PersonService { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthState { get; set; }

        public LastChangeModel LastChange = new LastChangeModel();
        private Persona CurrentUser;

        private async Task Save()
        {
            LastChange.FechaHora = DateTime.Now;
            LastChange.Responsable = CurrentUser;
            LastChange.CodigoIncidente = Incident.Code;
            await IncidentService.UpdateLastChange(LastChange);
            await OnSave.InvokeAsync(Incident);
        }

        private async Task SaveDestination(DestinationModel model)
        {
            Incident.Destination = model.Destination;
            LastChange.UltimoCambio = "Destino";
            await Save();
        }

        private async Task SaveOrigin(OriginModel model)
        {
            Incident.Origin = model.Origin;
            LastChange.UltimoCambio = "Origen";
            await Save();
        }

        private async Task SavePatient(PatientModel model)
        {
            Incident.MedicalRecord = model.Expediente;
            LastChange.UltimoCambio = "Paciente";
            await Save();
        }

        private async Task SaveAssignment(AssignmentModel model)
        {
            await AssignmentService.AssignToIncidentAsync(Incident.Code, model);
            LastChange.UltimoCambio = "Asignación";
            LastChange.FechaHora = DateTime.Now;
            LastChange.Responsable = CurrentUser;
            LastChange.CodigoIncidente = Incident.Code;
            await IncidentService.UpdateLastChange(LastChange);
        }

        protected async override Task OnInitializedAsync()
        {
            var emailUser = (await AuthState).User.Identity.Name;
            CurrentUser = await UserService.getPersonWithDetailstAsync(emailUser);
        }
    }
}