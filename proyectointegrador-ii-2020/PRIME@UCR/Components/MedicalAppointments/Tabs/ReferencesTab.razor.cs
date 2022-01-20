using BlazorTable;
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

    public partial class ReferencesTab
    {

        [Parameter] public CitaMedica Medical_Appointment { get; set; }

        [Parameter] public Cita Appointment { get; set; }

        [Parameter] public Paciente Pacient { get; set; }
        public RecordSummary Summary;

        private EditContext _context;

        public List<EspecialidadMedica> EspecialidadesMedicas;

        public EspecialidadMedica selected_specialty { get; set; }

        public bool already_selected { get; set; } = false;

        public List<Persona> Doctors { get; set; }

        public Persona selected_doctor { get; set; }

        public DateTime date { get; set; }

        public DateTime minTime { get; set; }

        public DateTime time { get; set; }

        public bool data_inserted { get; set; } = false;

        public bool unfinished_data = false;

        public bool reference_done = false;

        public List<ReferenceModel> references { get; set; }

        public ITable<ReferenceModel> ReferenceModelModel { get; set; }

        public bool addingreference { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            minTime = DateTime.Now;
            Summary = new RecordSummary();
            Summary.LoadPatientValues(Pacient);
            //references = new List<ReferenceModel>();
            //ReferenceModelModel = new ITable<Referen>
            EspecialidadesMedicas = (await appointment_service.GetMedicalSpecialtiesAsync()).ToList();
            _context = new EditContext(EspecialidadesMedicas);


            List<ReferenciaCita> appreferences = (await appointment_service.GetReferencesByAppId(Appointment.Id)).ToList();

            addingreference = true;
            await get_references(appreferences);
            addingreference = false;
            //StateHasChanged();
        }


        public string get_msgname()
        {
            return "Seleccione el médico (" + selected_specialty.Nombre + ")";
        }

        public async Task OnChangeSpecialty(EspecialidadMedica specialty)
        {
            Doctors = null;
            already_selected = true;
            selected_specialty = specialty;
            Doctors = (await appointment_service.GetDoctorsBySpecialtyNameAsync(specialty.Nombre)).ToList();
        }


        public async Task scheduleAppo()
        {
            reference_done = false;
            unfinished_data = false;

            if (selected_doctor != null && selected_specialty != null && date != null)
            {
                await schedule();
                selected_doctor = null;
                selected_specialty = null;
            }
            else
            {
                unfinished_data = true;
            }


        }


        public async Task schedule()
        {

            addingreference = true;

            string temp_date = date.ToString("dd MMMM yyyy hh:mm tt");

            Cita newapp = new Cita()
            {
                FechaHoraCreacion = DateTime.Now,
                FechaHoraEstimada = Convert.ToDateTime(temp_date),
                IdExpediente = Medical_Appointment.ExpedienteId
            };

            Cita cita = await appointment_service.InsertAppointmentAsync(newapp);

            CitaMedica newmedapp = new CitaMedica()
            {
                EstadoId = 7,
                CitaId = cita.Id,
                CedMedicoAsignado = selected_doctor.Cédula,
                ExpedienteId = Medical_Appointment.ExpedienteId,
                CentroMedicoId = Medical_Appointment.CentroMedicoId,
                Codigo = cita.FechaHoraCreacion.Year.ToString() + "-"
                      + cita.FechaHoraCreacion.Month.ToString() + "-" +
                       cita.FechaHoraCreacion.Day.ToString() + "-" + cita.Id + "-CM"
            };

            await appointment_service.InsertMedicalAppointmentAsync(newmedapp);

            ReferenciaCita reference = new ReferenciaCita()
            {
                IdCita1 = Appointment.Id,
                IdCita2 = cita.Id,
                Especialidad = selected_specialty.Nombre
            };

            //await appointment_service.InsertAppointmentReference(reference);

            await appointment_service.InsertAppointmentReference(reference);
            reference_done = true;
            already_selected = false;


            List<ReferenciaCita> appreferences = (await appointment_service.GetReferencesByAppId(Appointment.Id)).ToList();

            await get_references(appreferences);
            addingreference = false;
            StateHasChanged();
        }


        public async Task get_references(List<ReferenciaCita> appointmentreferences)
        {

            List<ReferenceModel> temp = new List<ReferenceModel>();

            for (int index = 0; index < appointmentreferences.Count; ++index)
            {

                Cita apptemp = await appointment_service.GetAppointmentByKeyAsync(appointmentreferences[index].IdCita2);

                if (apptemp != null)
                {
                    ReferenceModel current_reference = new ReferenceModel()
                    {
                        reference = appointmentreferences[index],
                        appointment = apptemp
                    };

                    temp.Add(current_reference);
                }
            }

            references = temp;
        }
    }
}
