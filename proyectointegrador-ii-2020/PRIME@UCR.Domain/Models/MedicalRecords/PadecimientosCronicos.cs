using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.MedicalRecords
{
    public class PadecimientosCronicos
    {

        public int IdExpediente { set; get; }  //Foreign key a Expediente, parte de la primary key

        public int IdListaPadecimiento { set; get; }  //Foreign key a ListaAlergia, parte de la primary key

        public DateTime FechaCreacion { get; set; }

        public Expediente Expediente { set; get; }

        public ListaPadecimiento ListaPadecimiento { set; get; }
    }
}
