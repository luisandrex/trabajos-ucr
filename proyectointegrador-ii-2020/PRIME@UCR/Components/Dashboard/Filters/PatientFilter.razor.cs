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
    public partial class PatientFilter
    {
        [Parameter] public AppointmentFilterModel Value { get; set; }
        [Parameter] public EventCallback<AppointmentFilterModel> ValueChanged { get; set; }
        [Parameter] public DashboardDataModel Data { get; set; }
        [Parameter] public EventCallback<DashboardDataModel> DataChanged { get; set; }

        private List<Paciente> _patients;
        private bool _isLoading = true;
        private bool _changesMade = false;

        private void OnChangePatient(Paciente patient)
        {
            if (Value.PatientModel.Find((p) => p.Patient == patient) != null)
            {
                _changesMade = false;
            }
            else
            {
                _changesMade = true;
            }
            Value._selectedPatientModel.Patient = patient;
        }

        private void LoadExistingValues()
        {
            _isLoading = true;
            StateHasChanged();
            _patients = Data.patients;
            _isLoading = false;
        }

        protected override void OnInitialized()
        {
            LoadExistingValues();
        }

        private void Discard()
        {
            _changesMade = false;
            Value._selectedPatientModel.Patient = Value.PatientModel.Last().Patient;
        }

        private async Task Save()
        {
            StateHasChanged();
            if (Value._selectedPatientModel == null)
            {
                Value.PatientModel.Clear();
            }
            else
            {
                PatientModel patientModel = new PatientModel { Patient = Value._selectedPatientModel.Patient };
                Value.PatientModel.Add(patientModel);
            }
            if (Value.PatientModel.Count() != 0)
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