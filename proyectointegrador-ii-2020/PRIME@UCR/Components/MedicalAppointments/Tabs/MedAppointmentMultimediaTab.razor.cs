using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Application.Services.Multimedia;
using PRIME_UCR.Application.Services.Appointments;
using PRIME_UCR.Components.MedicalRecords.Constants;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Components.MedicalAppointments.Tabs
{
    public partial class MedAppointmentMultimediaTab
    {

        [Inject] public IMultimediaContentService multimedia_content_service { get; set; }

        [Inject] public IAppointmentService appointment_service { get; set; }

        //[Parameter] public CitaMedica medical_appointment { get; set; }

        [Parameter] public string MedicalAppointmentId { get; set; }

        [Parameter] public Paciente Pacient { get; set; }
        public RecordSummary Summary;

        public CitaMedica medical_appointment { get; set; }

        public List<TipoAccion> _actionTypes { get; set; }
        public List<List<MultimediaContent>> _existingFiles { get; set; }

        public bool _isLoading { get; set; } = true;


        protected override async Task OnInitializedAsync()
        {
            Summary = new RecordSummary();
            Summary.LoadPatientValues(Pacient);
            medical_appointment = await appointment_service.GetMedicalAppointmentByKeyAsync(Convert.ToInt32(MedicalAppointmentId));  

            _actionTypes =
                (await appointment_service.GetActionsTypesMedicalAppointmentAsync())
                .ToList();
            _existingFiles = new List<List<MultimediaContent>>();
            foreach (var i in _actionTypes)
            {
                var content =
                    (await multimedia_content_service.GetByAppointmentAction(medical_appointment.CitaId, i.Nombre))
                    .ToList();
                _existingFiles.Add(content);
            }
            _isLoading = false;
        }

        private async Task OnFileUpload(TipoAccion action, MultimediaContent mc)
        {
            _isLoading = true;
            StateHasChanged();

            var i = _actionTypes.IndexOf(action);
            await multimedia_content_service.AddMultContToAction(medical_appointment.CitaId, action.Nombre, mc.Id);
            _existingFiles[i].Add(mc);

            _isLoading = false;
        }
    }
}
