using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Components.Incidents.IncidentDetails.Constants;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Components.Incidents.IncidentDetails.Tabs
{
    public partial class DestinationTab
    {
        [Inject] private ILocationService LocationService { get; set; }
        [Inject] private IDoctorService DoctorService { get; set; }
        [Inject] private IIncidentService IncidentService { get; set; }
        [Parameter] public IncidentDetailsModel Incident { get; set; }
        public Ubicacion Destination { get; set; }
        [Parameter] public EventCallback<DestinationModel> OnSave { get; set; }
        [Parameter] public string StatusMessage { get; set; }
        [Parameter] public string StatusClass { get; set; }
        [CascadingParameter] public Pages.Incidents.IncidentDetails ParentPage { get; set; }
        private DestinationModel _model = new DestinationModel();
        private MedicalCenterLocationModel _medicalCenterModel = new MedicalCenterLocationModel();
        private bool _isLoading = false;
        // Info for Incident summary that is shown at top of the page
        public IncidentSummary Summary = new IncidentSummary();
        private Estado _state = new Estado();
        private bool ReadOnly;

        private async Task OnMedicalCenterSave(MedicalCenterLocationModel medicalCenter)
        {
            _model.Destination = new CentroUbicacion
            {
                CedulaMedico = medicalCenter.Doctor.Cédula,
                CentroMedicoId = medicalCenter.MedicalCenter.Id,
                NumeroCama = medicalCenter.BedNumber
            };

            _medicalCenterModel = medicalCenter;
            await Save();
        }

        private async Task Save()
        {
            await OnSave.InvokeAsync(_model);
        }

        private void CheckReadOnly()
        {
            if (_state.Nombre == "Finalizado")
                ReadOnly = true;
        }

        private async Task LoadExistingValues()
        {
            _isLoading = true;
            Destination = Incident.Destination;
            _state = await IncidentService.GetIncidentStateByIdAsync(Incident.Code);
            CheckReadOnly();
            StateHasChanged();
            if (Destination is CentroUbicacion mc)
            {
                var doctor = await DoctorService.GetDoctorByIdAsync(mc.CedulaMedico);
                var medicalCenter = await LocationService.GetMedicalCenterById(mc.CentroMedicoId);
                _medicalCenterModel = new MedicalCenterLocationModel
                {
                    IsOrigin = false,
                    BedNumber = mc.NumeroCama,
                    Doctor = doctor,
                    MedicalCenter = medicalCenter
                };
            }
            _model.Destination = Destination;
            ParentPage.ClearStatusMessage();
            _isLoading = false;
        }

        protected override async Task OnInitializedAsync()
        {
            Summary.LoadValues(Incident);
            await LoadExistingValues();
        }
    }
}
