using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.MedicalRecords
{
    public class ListaPadecimiento
    {
        public int Id { get; set; }
        public string NombrePadecimiento { get; set; }
        public List<PadecimientosCronicos> PadecimientosCronicos { get; set; }
    }
}
