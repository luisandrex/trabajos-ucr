using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.UserAdministration
{
    public class NewPasswordModel
    {
        public NewPasswordModel()
        {
            Password = String.Empty;
            ConfirmedPassword = String.Empty;
        }

        public string Password { get; set; }

        public string ConfirmedPassword { get; set; }
    }
}
