using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PRIME_UCR.Application.DTOs.UserAdministration
{
    public class ChangePasswordModel
    {
        public ChangePasswordModel()
        {
            OldPassword = String.Empty;
            NewPassword = String.Empty;
            NewPasswordConfirm = String.Empty;
        }

        [Required(ErrorMessage = "Digite su contraseña actual")]
        [DataType(DataType.Password, ErrorMessage = "Digite su contraseña actual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Digite su nueva contraseña")]
        [DataType(DataType.Password, ErrorMessage = "Digite su nueva contraseña")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirme su nueva contraseña")]
        [DataType(DataType.Password, ErrorMessage = "Confirme su nueva contraseña")]
        public string NewPasswordConfirm { get; set; }
    }
}
