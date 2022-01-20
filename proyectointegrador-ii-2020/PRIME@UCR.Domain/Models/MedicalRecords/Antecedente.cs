using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PRIME_UCR.Domain.Models.MedicalRecords
{
    public class Antecedentes
    {
        public int IdExpediente { get; set; }
        public int IdListaAntecedentes { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Expediente Expediente { get; set; }
        public ListaAntecedentes ListaAntecedentes { get; set; }
    }
}
