using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.Implementations.Incidents;
using PRIME_UCR.Application.Services.Appointments;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Components.Controls;
using PRIME_UCR.Components.Incidents.IncidentDetails.Constants;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Components.Incidents.IncidentDetails.Tabs
{
    public partial class PatientTab
    {
        [Inject] private IPatientService PatientService { get; set; }
        [Inject] private IPersonService PersonService { get; set; }
        [Inject] private IMedicalRecordService MedicalRecordService { get; set; }
        [Inject] private IAppointmentService AppointmentService { get; set; }
        [Inject] private IAssignmentService AssignmentService { get; set; }
        [Inject] private IIncidentService IncidentService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject]
        public IUserService UserService { get; set; }
        [Parameter] public IncidentDetailsModel Incident { get; set; }
        [Parameter] public EventCallback<PatientModel> OnSave { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationState { get; set; }

        // Info for Incident summary that is shown at top of the page
        public IncidentSummary Summary = new IncidentSummary();
        // Finished state atributes
        private Estado _state = new Estado();
        private bool ReadOnly;

        private Gender? SelectedGender
        {
            get => _genders.FirstOrDefault(g =>
                       g?.ToString().First().ToString() == _model.Patient.Sexo?.First().ToString());
            set => _model.Patient.Sexo = value?.ToString().First().ToString();
        }

        private EditContext _context;
        private EditContext _patientContext;
        private PatientModel _model = new PatientModel();
        private PatientStatus _patientStatus;
        private bool _isLoading = true;
        private string _statusMessage = "";
        private readonly List<Gender?> _genders = new List<Gender?>();

        enum PatientStatus
        {
            PatientUnchanged,
            PatientExists,
            PersonExists,
            NewPerson
        }

        private bool IsReadOnly()
        {
            return _patientStatus != PatientStatus.NewPerson;
        }

        private async Task OnIdChange(string id)
        {
            _isLoading = true;
            StateHasChanged();
            _statusMessage = "";
            _model.CedPaciente = id;
            if (_context.Validate())
            {
                // if valid
                var patient = await PatientService.GetPatientByIdAsync(_model.CedPaciente);
                if (patient != null)
                {
                    _model.Patient = patient;
                    if (Incident.MedicalRecord?.CedulaPaciente == _model.CedPaciente)
                    {
                        _patientStatus = PatientStatus.PatientUnchanged;
                        _context = new EditContext(_model);
                    }
                    else
                    {
                        _patientStatus = PatientStatus.PatientExists;
                    }
                }
                else // check for person
                {
                    var person = await PersonService.GetPersonByIdAsync(id);
                    if (person != null)
                    {
                        // warn: existing person, but not a patient yet
                        _model.Patient = new Paciente
                        {
                            Cédula = person.Cédula,
                            FechaNacimiento = person.FechaNacimiento,
                            Nombre = person.Nombre,
                            PrimerApellido = person.PrimerApellido,
                            SegundoApellido = person.SegundoApellido,
                            Sexo = person.Sexo
                        };
                        _patientStatus = PatientStatus.PersonExists;
                    }
                    else
                    {
                        // new patient
                        _model.Patient = new Paciente
                        {
                            Cédula = _model.CedPaciente
                        };
                        _patientStatus = PatientStatus.NewPerson;
                    }
                }
                _patientContext = new EditContext(_model.Patient);
            }
            else
            {
                // invalid model, hide UI
                _model.Patient = null;
            }

            _isLoading = false;
        }

        private void CheckReadOnly()
        {
            if (_state.Nombre == "Finalizado")
                ReadOnly = true;
        }

        private async Task LoadExistingValues()
        {
            _isLoading = true;
            Summary.LoadValues(Incident);
            _state = await IncidentService.GetIncidentStateByIdAsync(Incident.Code);
            CheckReadOnly();
            StateHasChanged();
            _genders.AddRange(Enum.GetValues(typeof(Gender)).Cast<Gender?>());
            if (Incident.MedicalRecord != null)
            {
                _model.Patient = await PatientService.GetPatientByIdAsync(Incident.MedicalRecord.CedulaPaciente);
                _model.CedPaciente = _model.Patient.Cédula;
                _patientStatus = PatientStatus.PatientUnchanged;
                _patientContext = new EditContext(_model.Patient);
            }

            _context = new EditContext(_model);
            _statusMessage = "";
            _isLoading = false;
        }

        protected override async Task OnInitializedAsync()
        {
            Summary.LoadValues(Incident);
            _context = new EditContext(_model);

            await LoadExistingValues();

            _isLoading = false;
        }

        private async Task InsertPatient()
        {
            switch (_patientStatus)
            {
                case PatientStatus.PatientExists:
                    // already in DB
                    return;
                case PatientStatus.PersonExists:
                    _model.Patient = await PatientService.InsertPatientOnlyAsync(_model.Patient);
                    return;
                case PatientStatus.NewPerson:
                    await PatientService.CreatePatientAsync(_model.Patient);
                    return;
            }
        }

        private async Task Submit()
        {
            _isLoading = true;
            StateHasChanged();
            await InsertPatient();
            _model.Expediente =
                await AppointmentService.AssignMedicalRecordAsync(Incident.AppointmentId, _model.Patient);
            await OnSave.InvokeAsync(_model);
            _context = new EditContext(_model);
            _patientContext = new EditContext(_model.Patient);
            _patientStatus = PatientStatus.PatientUnchanged;
            _statusMessage = "Se guardaron los cambios exitosamente.";
            _isLoading = false;
        }
    }
}
