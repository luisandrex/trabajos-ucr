using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PRIME_UCR.Domain.Models.Appointments
{
    public class CitaMedica
    {
        public int Id { get; set; }
        public int EstadoId { get; set; } //fk-EstadoCitaMedica

        public int CitaId { get; set; } //fk-cita

        public string Codigo { get; set; }

        public string CedMedicoAsignado { get; set; } //fk-medico

        public int CentroMedicoId { get; set; } //fk-centro medico

        public int ExpedienteId { get; set; }

        public List<PoseeReceta> Recetas { get; set; }

        public Cita Cita { get; set; }

        public Médico Medico { get; set; }

        public CentroMedico medical_center { get; set; }

    }
}