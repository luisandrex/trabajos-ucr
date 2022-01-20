using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Text;

namespace PRIME_UCR.Domain.Models
{
    public class MultimediaContent
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Archivo { get; set; }

        public string Descripcion { get; set; }

        public DateTime Fecha_Hora { get; set; }

        public string Tipo { get; set; }

        public string Extension { get; set; }

    }
}
