using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.DTOs.Dashboard;

namespace PRIME_UCR.Components.Dashboard.Filters
{
    public partial class DestinationFilter
    {
        [Parameter] public FilterModel Value { get; set; }
        [Parameter] public EventCallback<FilterModel> ValueChanged { get; set; }
        [Parameter] public DashboardDataModel Data { get; set; }
        [Parameter] public EventCallback<DashboardDataModel> DataChanged { get; set; }

        private List<CentroMedico> _medicalCenters;
        private bool _isLoading = true;
        private bool _changesMade = false;
        
        private void OnChangeMedicalCenter(CentroMedico medicalCenter)
        {
            if(medicalCenter == Value.MedicalCenterDestination.MedicalCenter)
            {
                _changesMade = false;
            }
            else
            {
                _changesMade = true;
            }
            Value._selectedMedicalCenterDestination.MedicalCenter = medicalCenter;
        }

        private void LoadExistingValues()
        {
            _isLoading = true;
            StateHasChanged();
            _medicalCenters = Data.medicalCenters;
            _isLoading = false;
        }

        protected override void OnInitialized()
        {
            LoadExistingValues();
        }

        private void Discard()
        {
            _changesMade = false;
            Value._selectedMedicalCenterDestination.MedicalCenter = Value.MedicalCenterDestination.MedicalCenter;
        }

        private async Task Save()
        {
            StateHasChanged();
            Value.MedicalCenterDestination.MedicalCenter = Value._selectedMedicalCenterDestination.MedicalCenter;
            if (Value.MedicalCenterDestination.MedicalCenter != null)
            {
                Value.ButtonEnabled = true;
            }
            else
            {
                Value.ButtonEnabled = false;
            }
            _changesMade = false;
            await ValueChanged.InvokeAsync(Value);
        }

    }
}