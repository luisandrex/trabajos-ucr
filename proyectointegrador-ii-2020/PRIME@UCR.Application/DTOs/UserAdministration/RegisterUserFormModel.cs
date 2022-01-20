using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PRIME_UCR.Application.DTOs.UserAdministration
{
    public class RegisterUserFormModel
    {
        public RegisterUserFormModel()
        {
            Profiles = new List<string>();
        }

        public string IdCardNumber { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string FirstLastName { get; set; }

        public string? SecondLastName { get; set; }

        public char? Sex { get; set; }

        public DateTime? BirthDate { get; set; }

        public string PrimaryPhoneNumber { get; set; }

        public string SecondaryPhoneNumber { get; set; }

        public List<string> Profiles { get; set; }
    }
}
