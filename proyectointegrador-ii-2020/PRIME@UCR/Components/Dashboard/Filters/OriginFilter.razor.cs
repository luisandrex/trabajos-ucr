using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.Dashboard;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Components.Controls;
using PRIME_UCR.Components.Incidents.LocationPickers;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Components.Dashboard.Filters
{
    // Enum with the options for available origin types
    enum OriginType
    {
        [Description("Domicilio")]
        Household,
        [Description("Internacional")]
        International,
        [Description("Centro médico")]
        MedicalCenter
    }

    public partial class OriginFilter
    {
        [Parameter] public FilterModel Value { get; set; }
        [Parameter] public EventCallback<FilterModel> ValueChanged { get; set; }
        [Parameter] public EventCallback OnDiscard { get; set; }
        [Parameter] public DashboardDataModel Data { get; set; }
        [Parameter] public EventCallback<DashboardDataModel> DataChanged { get; set; }

        // Origin needed attributes

        private readonly List<string> _dropdownValuesOrigin = new List<string>
        {
            EnumUtils.GetDescription(OriginType.Household),
            EnumUtils.GetDescription(OriginType.International),
            EnumUtils.GetDescription(OriginType.MedicalCenter)
        };

        // Household needed attributes

        private List<Provincia> _provinces;
        private List<Canton> _cantons;
        private List<Distrito> _districts;

        // International needed attributes

        private List<Pais> _countries;

        // Medical centers needed attributes

        private List<CentroMedico> _medicalCenters;

        private bool _isLoading = true;
        private bool _changesMade = false;

        private void OnOriginTypeChange(string origin)
        {
            Value._selectedOriginType = origin;

            if (Value._selectedOriginType == null)
            {
                Value._selectedHouseholdOrigin.District = null;
                Value._selectedHouseholdOrigin.Canton = null;
                Value._selectedHouseholdOrigin.Province = null;
                Value._selectedMedicalCenterOrigin.MedicalCenter = null;
                Value._selectedInternationalOrigin.Country = null;
                if(Value.OriginType == null)
                {
                    _changesMade = false;
                }
                else
                {
                    _changesMade = true;
                }
            }
            else if (Value._selectedOriginType == "Domicilio") 
            {
                Value._selectedInternationalOrigin.Country = null;
                Value._selectedMedicalCenterOrigin.MedicalCenter = null;
                if (Value.OriginType == "Domicilio" &&
                    Value.HouseholdOriginFilter.Province == null &&
                    Value.HouseholdOriginFilter.Canton == null &&
                    Value.HouseholdOriginFilter.District == null)
                {
                    _changesMade = false;
                }
                else
                {
                    _changesMade = true;
                }
            }
            else if (Value._selectedOriginType == "Internacional")
            {
                Value._selectedHouseholdOrigin.District = null;
                Value._selectedHouseholdOrigin.Canton = null;
                Value._selectedHouseholdOrigin.Province = null;
                Value._selectedMedicalCenterOrigin.MedicalCenter = null;
                if (Value.OriginType == "Internacional")
                {
                    _changesMade = false;
                }
                else
                {
                    _changesMade = true;
                }
            }
            else
            {
                Value._selectedHouseholdOrigin.District = null;
                Value._selectedHouseholdOrigin.Canton = null;
                Value._selectedHouseholdOrigin.Province = null;
                Value._selectedInternationalOrigin.Country = null;
                if (Value.OriginType == "Centro médico" && 
                    Value.MedicalCenterOriginFilter.MedicalCenter == null)
                {
                    _changesMade = false;
                }
                else
                {
                    _changesMade = true;
                }
            }
        }

        private void LoadExistingValues()
        {
            _isLoading = true;
            StateHasChanged();
            
            //Initialize household attributes
            
            LoadProvinces(true);
            LoadCantons(true);
            LoadDistricts(true);

            //Initialize international attributes

            LoadCountries(true);

            //Initialize medical center attributes
            LoadMedicalCenters(true);

            _isLoading = false;
        }

        protected override void OnInitialized()
        {
            LoadExistingValues();
        }

        // Household needed methods

        void LoadProvinces(bool firstLoad)
        {
            _provinces = new List<Provincia>();
            foreach(var district in  Data.districts.GroupBy(d => d.Canton.NombreProvincia))
            {
                if(district.FirstOrDefault() != null && district.FirstOrDefault().Canton.Provincia.NombrePais == Pais.DefaultCountry)
                    _provinces.Add(district.First().Canton.Provincia);
            }

            if (!firstLoad)
                Value._selectedHouseholdOrigin.Province = null;
        }

        void OnChangeProvince(Provincia province)
        {
            StateHasChanged();
            Value._selectedHouseholdOrigin.Province = province;
            if (Value.HouseholdOriginFilter.Province == province &&
                Value.HouseholdOriginFilter.Canton == null &&
                Value.HouseholdOriginFilter.District == null)
            {
                _changesMade = false;
            }
            else
            {
                _changesMade = true;
            }
            LoadCantons(false);
            LoadDistricts(false);
        }

        void LoadCantons(bool firstLoad)
        {
            if (Value._selectedHouseholdOrigin.Province != null)
            {
                _cantons = new List<Canton>();
                foreach(var district in Data.districts.GroupBy(d => d.Canton.Nombre))
                {
                    if (district.FirstOrDefault() != null && district.FirstOrDefault().Canton.NombreProvincia == Value._selectedHouseholdOrigin.Province.Nombre)
                        _cantons.Add(district.FirstOrDefault().Canton);
                }
            }
            else
                _cantons = new List<Canton>();

            if (!firstLoad)
                Value._selectedHouseholdOrigin.Canton = null;
        }

        void OnChangeCanton(Canton canton)
        {
            StateHasChanged();
            Value._selectedHouseholdOrigin.Canton = canton;
            if (Value.HouseholdOriginFilter.Province == Value._selectedHouseholdOrigin.Province &&
                Value.HouseholdOriginFilter.Canton == canton &&
                Value.HouseholdOriginFilter.District == null)
            {
                _changesMade = false;
            }
            else
            {
                _changesMade = true;
            }
            LoadDistricts(false);
        }

        void LoadDistricts(bool firstLoad)
        {
            if (Value._selectedHouseholdOrigin.Canton != null)
            {
                _districts = new List<Distrito>();
                foreach(var district in Data.districts)
                {
                    if (district.IdCanton == Value._selectedHouseholdOrigin.Canton.Id)
                        _districts.Add(district);
                }
            }
            else
                _districts = new List<Distrito>();

            if (!firstLoad)
                Value._selectedHouseholdOrigin.District = null;
        }

        private void OnChangeDistrict(Distrito distrito)
        {
            StateHasChanged();
            Value._selectedHouseholdOrigin.District = distrito;
            if (Value.HouseholdOriginFilter.Province == Value._selectedHouseholdOrigin.Province &&
                Value.HouseholdOriginFilter.Canton == Value._selectedHouseholdOrigin.Canton &&
                Value.HouseholdOriginFilter.District == distrito)
            {
                _changesMade = false;
            }
            else
            {
                _changesMade = true;
            }
        }

        // Needed international methods 

        private void LoadCountries(bool firstRender) 
        {
            _countries = Data.countries.Where(c => c.Nombre != Pais.DefaultCountry).ToList();
            if (!firstRender)
                Value.InternationalOriginFilter.Country = null;
        }

        private void OnChangeCountry(Pais pais)
        {
            Value._selectedInternationalOrigin.Country = pais;
            if (Value.InternationalOriginFilter.Country == pais)
            {
                _changesMade = false;
            }
            else
            {
                _changesMade = true;
            }
        }

        // Needed medical centers methods
        void OnChangeMedicalCenter(CentroMedico medicalCenter)
        {
            Value._selectedMedicalCenterOrigin.MedicalCenter = medicalCenter;
            if (Value.MedicalCenterOriginFilter.MedicalCenter == medicalCenter)
            {
                _changesMade = false;
            }
            else
            {
                _changesMade = true;
            }
        }

        private void LoadMedicalCenters(bool firstRender)
        {
            _medicalCenters = Data.medicalCenters;
            if (!firstRender)
                Value._selectedMedicalCenterOrigin.MedicalCenter = null;
        }

        private void Discard()
        {
            _changesMade = false;
            Value._selectedOriginType = Value.OriginType;
            Value._selectedHouseholdOrigin.Province = Value.HouseholdOriginFilter.Province;
            Value._selectedHouseholdOrigin.Canton = Value.HouseholdOriginFilter.Canton;
            Value._selectedHouseholdOrigin.District = Value.HouseholdOriginFilter.District;
            Value._selectedInternationalOrigin.Country = Value.InternationalOriginFilter.Country;
            Value._selectedMedicalCenterOrigin.MedicalCenter = Value.MedicalCenterOriginFilter.MedicalCenter;
        }

        private async Task Save()
        {
            StateHasChanged();
            Value.OriginType = Value._selectedOriginType;
            Value.HouseholdOriginFilter.Province = Value._selectedHouseholdOrigin.Province;
            Value.HouseholdOriginFilter.Canton = Value._selectedHouseholdOrigin.Canton;
            Value.HouseholdOriginFilter.District = Value._selectedHouseholdOrigin.District;

            Value.InternationalOriginFilter.Country = Value._selectedInternationalOrigin.Country;

            Value.MedicalCenterOriginFilter.MedicalCenter = Value._selectedMedicalCenterOrigin.MedicalCenter;                
            if (Value.OriginType != null)
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