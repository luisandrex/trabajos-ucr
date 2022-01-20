using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Application.Implementations.MedicalRecords;

namespace PRIME_UCR.Components.MedicalRecords.Tabs
{
    public partial class MedicalBackgroundTab
    {
        [Parameter] public List<Antecedentes> Antecedentes { get; set; }
        [Parameter] public List<ListaAntecedentes> ListaAntecedentes { get; set; }
        [Parameter] public List<Alergias> Alergias { get; set; }
        [Parameter] public List<ListaAlergia> ListaAlergia { get; set; }
        [Parameter] public List<PadecimientosCronicos> PadecimientosCronicos { get; set; }
        [Parameter] public List<ListaPadecimiento> ListaPadecimiento { get; set; }
        [Parameter] public int idExpediente { get; set; }
        private EditContext _contAnte;
        private EditContext _contAle;
        private EditContext _contCond;

        public List<ListaAntecedentes> _backgroundList = new List<ListaAntecedentes>();
        public List<ListaAntecedentes> _currentBackgroundList = new List<ListaAntecedentes>();

        public List<ListaAlergia> _allergyList = new List<ListaAlergia>();
        public List<ListaAlergia> _currentAllergyList = new List<ListaAlergia>();

        public List<ListaPadecimiento> _conditionList = new List<ListaPadecimiento>();
        public List<ListaPadecimiento> _currentConditionList = new List<ListaPadecimiento>();

        public ListaAntecedentes antecedentePrueba;
        public ListaAlergia AlergiaPrueba;
        public ListaPadecimiento PadecimientoPrueba;

        private bool backgroundAlreadyAdded;
        private bool _saveBackgroundButtonEnabled;
        private bool _saveAllergyButtonEnabled;
        private bool _saveConditionButtonEnabled;

        private IEnumerable<int> RegisteredBackgrounds
        {
            get => _backgroundList.Select(bg => bg.Id);
            set =>
                   _backgroundList =
                        (from ante in ListaAntecedentes
                         join id in value on ante.Id equals id
                         select ante)
                        .ToList();
        }

        private IEnumerable<int> RegisteredAllergies
        {
            get => _allergyList.Select(bg => bg.Id);
            set =>
                   _allergyList =
                        (from ale in ListaAlergia
                         join id in value on ale.Id equals id
                         select ale)
                        .ToList();
        }

        private IEnumerable<int> RegisteredConditions
        {
            get => _conditionList.Select(bg => bg.Id);
            set =>
                   _conditionList =
                        (from cond in ListaPadecimiento
                         join id in value on cond.Id equals id
                         select cond)
                        .ToList();
        }

        MatTheme AddButtonTheme = new MatTheme()
        {
            Primary = "white",
            Secondary = "#095290"
        };

        protected override async Task OnInitializedAsync()
        {
            Antecedentes = (await MedicalBackgroundService.GetBackgroundByRecordId(idExpediente)).ToList();
            PadecimientosCronicos = (await ChronicConditionService.GetChronicConditionByRecordId(idExpediente)).ToList();
            Alergias = (await AllergyService.GetAlergyByRecordId(idExpediente)).ToList();

            LoadRecordBackgrounds();
            LoadAllergies();
            LoadConditions();
        }

        private async Task SaveMedicalBackground()
        {
            StateHasChanged();
            List<ListaAntecedentes> insertedList = new List<ListaAntecedentes>();
            ExceptBackgroundList(insertedList, _backgroundList, _currentBackgroundList);
            List<ListaAntecedentes> deletedList = new List<ListaAntecedentes>();
            ExceptBackgroundList(deletedList, _currentBackgroundList, _backgroundList);
            await MedicalBackgroundService.InsertBackgroundAsync(idExpediente, insertedList, deletedList);
            Antecedentes = (await MedicalBackgroundService.GetBackgroundByRecordId(idExpediente)).ToList();
            _contAnte = new EditContext(_backgroundList);
            _saveBackgroundButtonEnabled = false;
            _contAnte.OnFieldChanged += ToggleSaveBackgroundButton;
            LoadRecordBackgrounds();
        }

        private async Task SaveAllergies()
        {
            StateHasChanged();
            List<ListaAlergia> insertedList = new List<ListaAlergia>();
            ExceptAllergyList(insertedList, _allergyList, _currentAllergyList);
            List<ListaAlergia> deletedList = new List<ListaAlergia>();
            ExceptAllergyList(deletedList, _currentAllergyList, _allergyList);
            await AllergyService.InsertAllergyAsync(idExpediente, insertedList, deletedList);
            Alergias = (await AllergyService.GetAlergyByRecordId(idExpediente)).ToList();
            _contAle = new EditContext(_allergyList);
            _saveAllergyButtonEnabled = false;
            _contAle.OnFieldChanged += ToggleSaveAllergyButton;
            LoadAllergies();
        }

        private async Task SaveConditions()
        {
            StateHasChanged();
            List<ListaPadecimiento> insertedList = new List<ListaPadecimiento>();
            ExceptConditionList(insertedList, _conditionList, _currentConditionList);
            List<ListaPadecimiento> deletedList = new List<ListaPadecimiento>();
            ExceptConditionList(deletedList, _currentConditionList, _conditionList);
            await ChronicConditionService.InsertConditionAsync(idExpediente, insertedList, deletedList);
            PadecimientosCronicos = (await ChronicConditionService.GetChronicConditionByRecordId(idExpediente)).ToList();
            _contCond = new EditContext(_conditionList);
            _saveConditionButtonEnabled = false;
            _contCond.OnFieldChanged += ToggleSaveConditionButton;
            LoadConditions();
        }

        private void ToggleSaveBackgroundButton(object? sender, FieldChangedEventArgs e)
        {
            _saveBackgroundButtonEnabled = _contAnte.IsModified();
            StateHasChanged();
        }

        private void ToggleSaveAllergyButton(object? sender, FieldChangedEventArgs e)
        {
            _saveAllergyButtonEnabled = _contAle.IsModified();
            StateHasChanged();
        }

        private void ToggleSaveConditionButton(object? sender, FieldChangedEventArgs e)
        {
            _saveConditionButtonEnabled = _contCond.IsModified();
            StateHasChanged();
        }

        private void ExceptBackgroundList(List<ListaAntecedentes> returnList, List<ListaAntecedentes> inputList, List<ListaAntecedentes> exceptionList)
        {
            bool stop = false;
            foreach (ListaAntecedentes background in inputList) {
                for (int i = 0; i < exceptionList.Count() && !stop; ++i)
                {
                    if (background.Id == exceptionList[i].Id)
                    {
                        stop = true;
                    }
                }
                if (!stop) {
                    returnList.Add(background);
                }
                stop = false;
            }
        }

        private void ExceptAllergyList(List<ListaAlergia> returnList, List<ListaAlergia> inputList, List<ListaAlergia> exceptionList)
        {
            bool stop = false;
            foreach (ListaAlergia allergy in inputList)
            {
                for (int i = 0; i < exceptionList.Count() && !stop; ++i)
                {
                    if (allergy.Id == exceptionList[i].Id)
                    {
                        stop = true;
                    }
                }
                if (!stop)
                {
                    returnList.Add(allergy);
                }
                stop = false;
            }
        }

        private void ExceptConditionList(List<ListaPadecimiento> returnList, List<ListaPadecimiento> inputList, List<ListaPadecimiento> exceptionList)
        {
            bool stop = false;
            foreach (ListaPadecimiento condition in inputList)
            {
                for (int i = 0; i < exceptionList.Count() && !stop; ++i)
                {
                    if (condition.Id == exceptionList[i].Id)
                    {
                        stop = true;
                    }
                }
                if (!stop)
                {
                    returnList.Add(condition);
                }
                stop = false;
            }
        }

        private async Task LoadRecordBackgrounds()
        {
            _contAnte = new EditContext(_backgroundList);
            _saveBackgroundButtonEnabled = false;
            _contAnte.OnFieldChanged += ToggleSaveBackgroundButton;
            _backgroundList.Clear();
            _currentBackgroundList.Clear();
            foreach (Antecedentes background in Antecedentes)
            {
                _backgroundList.Add(background.ListaAntecedentes);
                _currentBackgroundList.Add(background.ListaAntecedentes);
            }
        }

        private async Task LoadAllergies()
        {
            _contAle = new EditContext(_allergyList);
            _saveAllergyButtonEnabled = false;
            _contAle.OnFieldChanged += ToggleSaveAllergyButton;
            _allergyList.Clear();
            _currentAllergyList.Clear();
            foreach (Alergias allergy in Alergias)
            {
                _allergyList.Add(allergy.ListaAlergia);
                _currentAllergyList.Add(allergy.ListaAlergia);
            }
        }

        private async Task LoadConditions()
        {
            _contCond = new EditContext(_conditionList);
            _saveConditionButtonEnabled = false;
            _contCond.OnFieldChanged += ToggleSaveConditionButton;
            _conditionList.Clear();
            _currentConditionList.Clear();
            foreach (PadecimientosCronicos condition in PadecimientosCronicos)
            {
                _conditionList.Add(condition.ListaPadecimiento);
                _currentConditionList.Add(condition.ListaPadecimiento);
            }
        }

        public void Dispose()
        {
            if (_contAnte != null)
                _contAnte.OnFieldChanged -= ToggleSaveBackgroundButton;
            if (_contAle != null)
                _contAle.OnFieldChanged -= ToggleSaveAllergyButton;
            if (_contCond != null)
                _contCond.OnFieldChanged -= ToggleSaveConditionButton;
        }

    }
}
