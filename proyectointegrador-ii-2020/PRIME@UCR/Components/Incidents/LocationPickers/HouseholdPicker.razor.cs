using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Internal;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Components.Controls;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Components.Incidents.LocationPickers
{
    public partial class HouseholdPicker : IDisposable
    {
        [Inject] public ILocationService LocationService { get; set; }
        [Parameter] public HouseholdModel Value { get; set; }
        [Parameter] public bool IsFirst { get; set; }
        [Parameter] public EventCallback<HouseholdModel> OnSave { get; set; }
        [Parameter] public EventCallback OnDiscard { get; set; }
        [Parameter] public bool ReadOnly { get; set; } = false;

        private List<Provincia> _provinces;
        private List<Canton> _cantons;
        private List<Distrito> _districts;
        private bool _saveButtonEnabled = false;
        private EditContext _context;
        
        private bool _isLoading = true;

        async Task LoadProvinces(bool firstLoad)
        {
            // get options
            _provinces =
                (await LocationService.GetProvincesByCountryNameAsync(Pais.DefaultCountry))
                .ToList();

            if (!firstLoad)
                Value.Province = null;
        }

        async Task OnChangeProvince(Provincia province)
        {
            Value.Province = province;
            await LoadCantons(false);
            await LoadDistricts(false);
        }

        async Task LoadCantons(bool firstLoad)
        {
            if (Value.Province != null)
                _cantons =
                    (await LocationService.GetCantonsByProvinceNameAsync(Value.Province.Nombre))
                    .ToList();
            else
                _cantons = new List<Canton>();

            if (!firstLoad)
                Value.Canton = null;
        }

        async Task OnChangeCanton(Canton canton)
        {
            Value.Canton = canton;
            await LoadDistricts(false);
        }

        async Task LoadDistricts(bool firstLoad)
        {
            if (Value.Canton != null)
                _districts =
                    (await LocationService.GetDistrictsByCantonIdAsync(Value.Canton.Id))
                    .ToList();
            else
                _districts = new List<Distrito>();

            if (!firstLoad)
                Value.District = null;
        }

        public async Task LoadExistingValues()
        {
            _isLoading = true;
            StateHasChanged(); 
            await LoadProvinces(true);
            await LoadCantons(true);
            await LoadDistricts(true);
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
                Value = new HouseholdModel();
            
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
