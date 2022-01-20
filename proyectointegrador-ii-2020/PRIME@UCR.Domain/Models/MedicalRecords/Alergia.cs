using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.MedicalRecords
{
    public class Alergias
    {

        public int IdExpediente { set; get; }  //Foreign key a Expediente, parte de la primary key

        public int IdListaAlergia { set; get; }  //Foreign key a ListaAlergia, parte de la primary key
        public DateTime FechaCreacion { get; set; }

        public Expediente Expediente { set; get; }

        public ListaAlergia ListaAlergia { set; get; }
    }
}
