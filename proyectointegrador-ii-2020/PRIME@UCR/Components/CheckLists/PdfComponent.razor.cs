using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using PRIME_UCR.Application.DTOs.CheckLists;
using PRIME_UCR.Application.DTOs.Incidents;
using PRIME_UCR.Application.Services.CheckLists;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Application.Services.Multimedia;
using Microsoft.JSInterop;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Application.Services.Appointments;

namespace PRIME_UCR.Components.CheckLists
{
    public partial class PdfComponent
    {
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private IPdfService MyPdfService { get; set; }
        [Inject] private IIncidentService MyIncidentService { get; set; }
        [Inject] private IChronicConditionService MyChronicConditionService { get; set; }
        [Inject] private IMedicalBackgroundService MyMedicalBackgroundService { get; set; }
        [Inject] private IAssignmentService MyAssignmentService { get; set; }
        [Inject] private IPatientService MyPatientService { get; set; }
        [Inject] public IMultimediaContentService MyMultimediaContentService { get; set; }
        [Inject] public IAppointmentService AppointmentService { get; set; }
        [Parameter] public string IncidentCode { get; set; }
        [Parameter] public string IncidentState { get; set; }
        [Parameter] public EventCallback<bool> ChangeLoading { get; set; }

        private bool Disabled { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            // Nothing in here for now.
        }
        protected async Task ExportToPdf()
        {
            await ChangeLoading.InvokeAsync(true);
            PdfModel pdfModel = new PdfModel();
            pdfModel.Incident = await MyIncidentService.GetIncidentDetailsAsync(IncidentCode);
            if (pdfModel.Incident.MedicalRecord != null)
            {
                pdfModel.Patient = await MyPatientService.GetPatientByIdAsync(pdfModel.Incident.MedicalRecord.CedulaPaciente);
                pdfModel.ChronicConditions = (await MyChronicConditionService.GetChronicConditionByRecordId(pdfModel.Incident.MedicalRecord.Id)).ToList();
                pdfModel.Background = (await MyMedicalBackgroundService.GetBackgroundByRecordId(pdfModel.Incident.MedicalRecord.Id)).ToList();
            }
            if (IncidentState != "Iniciado" && IncidentState != "Creado" && IncidentState != "Rechazado" && IncidentState != "Aprobado" && IncidentState != "En proceso de creación")
            {
                pdfModel.AssignedMembers = await MyAssignmentService.GetAssignmentsByIncidentIdAsync(IncidentCode);
            }

            // Multimedia content
            pdfModel.ActionTypes =
                (await AppointmentService.GetActionTypesAsync())
                .ToList();
            pdfModel.ExistingFiles = new List<List<MultimediaContent>>();
            foreach (var i in pdfModel.ActionTypes)
            {
                var content =
                    (await MyMultimediaContentService.GetByAppointmentAction(pdfModel.Incident.AppointmentId, i.Nombre))
                    .ToList();
                pdfModel.ExistingFiles.Add(content);
            }

            string fileName = "Reporte " + IncidentCode + ".pdf";
            using (MemoryStream excelStream = MyPdfService.GenerateIncidentPdf(pdfModel))
            {
                await JS.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(excelStream.ToArray()));
            }
            await ChangeLoading.InvokeAsync(false);
        }
    }
}
