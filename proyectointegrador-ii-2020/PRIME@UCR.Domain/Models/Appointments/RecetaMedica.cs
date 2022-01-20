using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PRIME_UCR.Domain.Models.Appointments
{
    public class RecetaMedica
    {
        public int Id { get; set; }

        public string NombreReceta { get; set; }

        public List<PoseeReceta> Recetas { get; set; }
        
    }
}
