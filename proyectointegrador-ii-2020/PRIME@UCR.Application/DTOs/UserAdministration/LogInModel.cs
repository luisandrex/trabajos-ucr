using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PRIME_UCR.Application.DTOs.UserAdministration
{
    public class LogInModel
    {
        [Required(ErrorMessage ="Digite su correo")]
        [EmailAddress(ErrorMessage = "Digite un correo válido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Digite su contraseña")]
        [DataType(DataType.Password, ErrorMessage = "Digite su contraseña")]
        public string Contraseña { get; set; }
    }
}
