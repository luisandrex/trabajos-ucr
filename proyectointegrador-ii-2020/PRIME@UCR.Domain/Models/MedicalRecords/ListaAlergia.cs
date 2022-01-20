using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.MedicalRecords
{
    public class ListaAlergia
    {
        public int Id { set; get; }

        public string NombreAlergia { set; get; }

        public List<Alergias> Alergias { set; get; }
    }
}
