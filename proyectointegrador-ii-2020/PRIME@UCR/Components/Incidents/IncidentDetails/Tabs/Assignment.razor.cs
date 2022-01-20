using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Components.Incidents.IncidentDetails.Constants;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Components.Incidents.IncidentDetails.Tabs
{
    public partial class Assignment
    {
        [Parameter] public IncidentDetailsModel Incident { get; set; }
        [Parameter] public EventCallback<AssignmentModel> OnSave { get; set; }
        [Inject] public IAssignmentService AssignmentService { get; set; }
        [Inject] private IIncidentService IncidentService { get; set; }


        private IEnumerable<string> Specialists
        {
            get => _model.TeamMembers.Select(tm => tm.Cédula);
            set =>
                _model.TeamMembers =
                    (from member in _specialists
                        join id in value on member.Cédula equals id
                        select member)
                    .ToList();
        }
        
        // dropdown menu values 
        private List<UnidadDeTransporte> _transportUnits;
        private List<EspecialistaTécnicoMédico> _specialists;
        private List<CoordinadorTécnicoMédico> _coordinators;
        
        private string _statusMessage = "";
        private AssignmentModel _model;
        private bool _isLoading = true;
        private bool _saveButtonEnabled;
        private EditContext _context;

        // Info for Incident summary that is shown at top of the page
        public IncidentSummary Summary = new IncidentSummary();
        //Finished state atributes
        private Estado _state = new Estado();
        private bool ReadOnly;
        private string CordinName;
        private string SpecialistsNames;


        private async Task Save()
        {
            _isLoading = true;
            StateHasChanged();
            await OnSave.InvokeAsync(_model);
            await AssignmentService.AssignToIncidentAsync(Incident.Code, _model);
            _statusMessage = "Se guardaron los cambios exitosamente.";
            _context = new EditContext(_model);
            _saveButtonEnabled = false;
            _context.OnFieldChanged += ToggleSaveButton;
            _isLoading = false;
        }

        private void LoadSpecialistsNames()
        {
            SpecialistsNames = "";
            for (int i=0; i < _specialists.Count; i++)
            {
                SpecialistsNames += _specialists[i].NombreCompleto;
                if(i < _specialists.Count -1)
                {
                    SpecialistsNames += ", ";
                }

            }
        }

        private void CheckReadOnly()
        {
            if (_state.Nombre == "Finalizado")
            {
                LoadSpecialistsNames();
                CordinName = _model.Coordinator.NombreCompleto;
                ReadOnly = true;
            }
        }


        private async Task LoadExistingValues()
        {
            Summary.LoadValues(Incident);
            // make sure it's initialized
            _model = await AssignmentService.GetAssignmentsByIncidentIdAsync(Incident.Code);
            _state = await IncidentService.GetIncidentStateByIdAsync(Incident.Code);
            _context = new EditContext(_model);
            _saveButtonEnabled = false;
            _context.OnFieldChanged += ToggleSaveButton;

            _transportUnits =
                (await AssignmentService.GetAllTransportUnitsByMode(Incident.Mode))
                .ToList();

            _coordinators =
                (await AssignmentService.GetCoordinatorsAsync())
                .ToList();

            _specialists =
                (await AssignmentService.GetSpecialistsAsync())
                .ToList();          
            CheckReadOnly();
            _statusMessage = "";
            _isLoading = false;
        }

        private void ToggleSaveButton(object? sender, FieldChangedEventArgs e)
        {
            _saveButtonEnabled = _context.IsModified();
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadExistingValues();
        }

        public void Dispose()
        {
            if (_context != null)
                _context.OnFieldChanged -= ToggleSaveButton;
        }
    }
}
