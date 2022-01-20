using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PRIME_UCR.Domain.Models
{
    public class Accion
    {
        public int CitaId { get; set; }
        public string NombreAccion { get; set; }
        public int MultContId { get; set; }
        public Cita Cita { get; set; }
        public TipoAccion TipoAccion { get; set; }
        public MultimediaContent MultimediaContent { get; set; }
    }
}
