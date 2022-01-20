using PRIME_UCR.Domain.Models.MedicalRecords;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PRIME_UCR.Domain.Models.Appointments
{
    public class PoseeReceta
    {

        public string Dosis { get; set; }   //descripcion de la dosis del medicamento.

        public int IdRecetaMedica { get; set; } //fk-RecetaMedica

        public int IdCitaMedica { get; set; } //fk-Expediente


        public RecetaMedica RecetaMedica { get; set; }

        public CitaMedica CitaMedica { get; set; }


    }
}
