using System;
using System.Collections.Generic;
using System.Linq;
using PRIME_UCR.Domain.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Domain.Constants;

namespace PRIME_UCR.Components.Incidents.IncidentDetails.Tabs
{
    public partial class InfoTab
    {
        [Parameter] public IncidentDetailsModel DetailsModel { get; set; }
        [Parameter] public EventCallback OnSave { get; set; }
        [Parameter] public Persona CurrentUser { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthState { get; set; }
        [Inject] public IPersonService PersonService { get; set; }
        [Inject] public IIncidentService IncidentService { get; set; }
        [Inject] public IUserService UserService { get; set; }

        private Persona _creator;
        private bool _isLoading = true;

        public bool showDeniedMessage = false;
        public string DeniedMessage;

        public List<string> values = new List<string>();
        public List<string> content = new List<string>();
        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            await checkDeniedState();
            _creator = await PersonService.GetPersonByIdAsync(DetailsModel.AdminId);
            _isLoading = false;
        }

        private async Task checkDeniedState()
        {
            if (DetailsModel.CurrentState == IncidentStates.Rejected.Nombre)
            {
                showDeniedMessage = true;
                IEnumerable<DocumentacionIncidente> enumerable = await IncidentService.GetAllDocumentationByIncidentCode(DetailsModel.Code);
                List<DocumentacionIncidente> lista = enumerable.ToList();
                lista = lista.OrderBy(i => i.Id).ToList();
                string feedBack = lista[0].RazonDeRechazo;
                string nombreReviewer = DetailsModel.Reviewer.NombreCompleto;
                content.Add("Rechazado por: ");
                values.Add(" " + nombreReviewer);
                content.Add("Razón de rechazo: ");
                values.Add(" " + feedBack);
            }
            else 
            {
                showDeniedMessage = false;
            }
        }

        public void IsLoading(bool loadingValue)
        {
            _isLoading = loadingValue;
        }
    }
}
