using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.Appointments
{
    public class AppointmentListModel
    {
        public DateTime FechaCreacion { get; set; }
        public int CitaId { get; set; }
        public int ExpedienteId { get; set; }
        public string Tipo { get; set; }
    }
}
