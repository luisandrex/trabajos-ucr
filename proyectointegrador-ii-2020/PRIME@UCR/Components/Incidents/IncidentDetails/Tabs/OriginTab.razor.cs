using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Components.Controls;
using PRIME_UCR.Components.Incidents.IncidentDetails.Constants;
using PRIME_UCR.Components.Incidents.LocationPickers;
using PRIME_UCR.Components.Incidents.IncidentDetails;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Components.Incidents.IncidentDetails.Tabs
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

    public partial class OriginTab
    {
        [Inject] private ILocationService LocationService { get; set; }
        [Inject] private IDoctorService DoctorService { get; set; }
        [Parameter] public IncidentDetailsModel Incident { get; set; }
        [Parameter] public EventCallback<OriginModel> OnSave { get; set; }
        [CascadingParameter] public Pages.Incidents.IncidentDetails ParentPage { get; set; }
        [Inject] public IIncidentService IncidentService { get; set; }
        [Parameter] public string StatusMessage { get; set; }
        [Parameter] public string StatusClass { get; set; }
        public Ubicacion Origin { get; set; }
        // Info for Incident summary that is shown at top of the page
        public IncidentSummary Summary = new IncidentSummary();
        // Selected options
        private Tuple<OriginType, string> _selectedOriginType;
        private OriginModel _model = new OriginModel();
        private HouseholdModel _householdModel = new HouseholdModel();
        private InternationalModel _internationalModel = new InternationalModel();
        private MedicalCenterLocationModel _medicalCenterModel = new MedicalCenterLocationModel();
        private Estado _state = new Estado();
        private string TypeOfOrigin;
        private bool ReadOnly;


        // Lists of options
        private readonly List<Tuple<OriginType, string>> _dropdownValuesOrigin = new List<Tuple<OriginType, string>>
        {
            Tuple.Create(OriginType.Household, EnumUtils.GetDescription(OriginType.Household)),
            Tuple.Create(OriginType.International, EnumUtils.GetDescription(OriginType.International)),
            Tuple.Create(OriginType.MedicalCenter, EnumUtils.GetDescription(OriginType.MedicalCenter))
        };

        private void OnOriginTypeChange(Tuple<OriginType, string> type)
        {
            ParentPage.ClearStatusMessage();
            _selectedOriginType = type;
        }

        private void OnOriginChange(Ubicacion origin)
        {
            _model.Origin = origin;
        }

        private bool _isLoading;

        private async Task OnHouseholdSave(HouseholdModel household)
        {
            if (household.Longitude != null && household.Latitude != null)
            {
                _model.Origin = new Domicilio
                {
                    Direccion = household.Address,
                    DistritoId = household.District.Id,
                    Longitud = (double) household.Longitude,
                    Latitud = (double) household.Latitude
                };
            }
            else
            {
                throw new ApplicationException("Household picker shouldn't return null longitude or latitude");
            }

            _householdModel = household;
            await Save();
        }

        private async Task OnInternationalSave(InternationalModel international)
        {
            _model.Origin = new Internacional
            {
                NombrePais = international.Country.Nombre
            };

            _internationalModel = international;
            await Save();
        }

        private async Task OnMedicalCenterSave(MedicalCenterLocationModel medicalCenter)
        {
            _model.Origin = new CentroUbicacion
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
            Origin = Incident.Origin;
            _state = await IncidentService.GetIncidentStateByIdAsync(Incident.Code);
            CheckReadOnly();
            StateHasChanged();
            switch (Origin)
            {
                case Domicilio d:
                {
                    _selectedOriginType = _dropdownValuesOrigin[0];
                    var location = await LocationService.GetLocationByDistrictId(d.DistritoId);
                    _householdModel = new HouseholdModel
                    {
                        Province = location.Province,
                        Canton = location.Canton,
                        District = location.District,
                        Address = d.Direccion,
                        Longitude = d.Longitud,
                        Latitude = d.Latitud
                    };
                    break;
                }
                case Internacional i:
                    _selectedOriginType = _dropdownValuesOrigin[1];
                    _internationalModel = new InternationalModel
                    {
                        Country = await LocationService.GetCountryByName(i.NombrePais)
                    };
                    break;
                case CentroUbicacion mc:
                    _selectedOriginType = _dropdownValuesOrigin[2];
                    var doctor = await DoctorService.GetDoctorByIdAsync(mc.CedulaMedico);
                    var medicalCenter = await LocationService.GetMedicalCenterById(mc.CentroMedicoId);
                    _medicalCenterModel = new MedicalCenterLocationModel
                    {
                        IsOrigin = true,
                        BedNumber = mc.NumeroCama,
                        Doctor = doctor,
                        MedicalCenter = medicalCenter
                    };
                    break;
                default:
                    _selectedOriginType = _dropdownValuesOrigin[0];
                    break;
            }
            _model.Origin = Origin;
            ParentPage.ClearStatusMessage();
            TypeOfOrigin = _selectedOriginType.Item2;
            _model.Origin = Origin;
            _isLoading = false;
        }

        protected override async Task OnInitializedAsync()
        {
            Summary.LoadValues(Incident);
            await LoadExistingValues();
        }
    }

}
