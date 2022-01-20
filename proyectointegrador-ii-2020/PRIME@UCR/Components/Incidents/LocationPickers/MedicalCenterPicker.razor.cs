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

namespace PRIME_UCR.Components.Incidents.LocationPickers
{
    public partial class MedicalCenterPicker
    {
        [Inject] public ILocationService LocationService { get; set; }
        [Parameter] public MedicalCenterLocationModel Value { get; set; }
        [Parameter] public bool IsOrigin { get; set; } 
        [Parameter] public EventCallback<MedicalCenterLocationModel> OnSave { get; set; }
        [Parameter] public EventCallback OnDiscard { get; set; }
        [Parameter] public bool IsFirst { get; set; }
        [Parameter] public bool ReadOnly { get; set; } = false;// If the elements in tab are readOnly or input required
        public string DoctorForLabel => IsOrigin ? "Médico en origen" : "Médico en destino";
        private string BedNumberText;//String conversion of integer bednumber
        public string DoctorFullName;

        private List<CentroMedico> _medicalCenters;
        private List<Médico> _doctors;
        private EditContext _context;
        private bool _saveButtonEnabled;
        private bool _isLoading = true;

        async Task OnChangeMedicalCenter(CentroMedico medicalCenter)
        {
            Value.MedicalCenter = medicalCenter;
            await LoadDoctors(false);
        }

        private async Task LoadMedicalCenters(bool firstRender)
        {
            _medicalCenters =
                (await LocationService.GetAllMedicalCentersAsync())
                .ToList();

            if (!firstRender)
                Value.MedicalCenter = null;
        }

        private async Task LoadDoctors(bool firstRender)
        {
            if (Value.MedicalCenter != null)
            {
                _doctors =
                    (await LocationService.GetAllDoctorsByMedicalCenter(Value.MedicalCenter.Id))
                    .ToList();
            }
            else
            {
                _doctors = new List<Médico>();
            }

            if (!firstRender)
            {
                Value.Doctor = null;
            }
        }

        private async Task LoadExistingValues()
        {
            _isLoading = true;
            BedNumberText = Value.BedNumber?.ToString();
            StateHasChanged();
            await LoadMedicalCenters(true);
            await LoadDoctors(true);
            DoctorFullName = Value.Doctor?.NombreCompleto;
            _isLoading = false;
        }
        
        private async Task Submit()
        {
            _isLoading = true;
            StateHasChanged();
            await OnSave.InvokeAsync(Value);
            _context.OnFieldChanged -= HandleFieldChanged;
            _context = new EditContext(Value);
            _context.OnFieldChanged += HandleFieldChanged;
            _saveButtonEnabled = false;
            _isLoading = false;
        }

        private async Task Discard()
        {
            await OnDiscard.InvokeAsync(null);
            if (!IsFirst && !OnDiscard.HasDelegate)
                await LoadExistingValues();
        }

        protected override async Task OnInitializedAsync()
        {
            if (IsFirst)
                Value = new MedicalCenterLocationModel { IsOrigin = IsOrigin };
            
            await LoadExistingValues();
            
            _saveButtonEnabled = IsFirst;
                
            _context = new EditContext(Value);
            _context.OnFieldChanged += HandleFieldChanged;
        }

        // used to toggle submit button disabled attribute
        private void HandleFieldChanged(object sender, FieldChangedEventArgs e)
        {
            _saveButtonEnabled = _context.IsModified();
            StateHasChanged();
        }

        public void Dispose()
        {
            if (_context != null)
                _context.OnFieldChanged -= HandleFieldChanged;
        }
    }
}