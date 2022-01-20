using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PRIME_UCR.Components.MedicalRecords.Constants;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.MedicalAppointments.Tabs
{
    public partial class MetricsTab
    { 
        [Parameter] public int AppointmentId { get; set; }

        [Parameter] public Paciente Pacient { get; set; }

        private EditContext MetricContext;

        public RecordSummary Summary;
  
        private MetricsApp MetricsForm { get; set; }

        private MetricasCitaMedica Metrics;

        private bool metrics_saved { get; set; } = false;

        private bool metrics_updated { get; set; } = false; 

        protected override async Task OnInitializedAsync() {
            MetricsForm = new MetricsApp();
            MetricContext = new EditContext(MetricsForm);
            Metrics = await appointment_service.GetMetricsMedAppointmentByAppId(AppointmentId);
            Summary = new RecordSummary();
            Summary.LoadPatientValues(Pacient);
            if (Metrics != null)
            {
                MetricsForm.Altura = Metrics.Altura.ToString();
                MetricsForm.Peso = Metrics.Peso.ToString();

                int index = 0;
                while (index < Metrics.Presion.Length && Metrics.Presion[index] != '/') {
                    ++index; 
                }

                if (Metrics.Presion[index] == '/')
                {
                    int length = Metrics.Presion.Length;
                    MetricsForm.PresionSys = Metrics.Presion.Substring(0, index);
                    MetricsForm.PresionDia = Metrics.Presion.Substring(index+1, length-(index+1));
                }
                else {
                    MetricsForm.PresionSys = "";
                    MetricsForm.PresionDia = ""; 
                }
            }
        
        }


        public async Task saveMetricData() {
            metrics_saved = false;
            metrics_updated = false; 

            Metrics = await appointment_service.GetMetricsMedAppointmentByAppId(AppointmentId);

            if (Metrics != null)
            {

                Metrics.Altura = MetricsForm.Altura;
                Metrics.Peso = MetricsForm.Peso;
                Metrics.Presion = MetricsForm.PresionSys + "/" + MetricsForm.PresionDia;

                await appointment_service.UpdateMetrics(Metrics);
                metrics_updated = true; 
            }
            else
            {
                Metrics = new MetricasCitaMedica()
                {
                    Altura = MetricsForm.Altura,
                    Peso = MetricsForm.Peso,
                    Presion = MetricsForm.PresionSys + "/" + MetricsForm.PresionDia,
                    CitaId = AppointmentId
                }; 
                await appointment_service.InsertMetrics(Metrics);
                metrics_saved = true;
            }
        }
    }
}




/*
 * 
 * INSERT INTO Cita(FechaHoraCreacion, FechaHoraEstimada, IdExpediente)
VALUES(GETDATE(), GETDATE(), 18)

INSERT INTO CitaMedica(ExpedienteId, CedMedicoAsignado, CentroMedicoId, EstadoId, CitaId)
VALUES(18, 22222222, 2, 7, 45)

 * 
 * */
