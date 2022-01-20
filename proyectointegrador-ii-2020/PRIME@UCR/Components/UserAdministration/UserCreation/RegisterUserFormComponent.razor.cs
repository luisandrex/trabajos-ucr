using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.DTOs.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.UserAdministration.UserCreation
{
    public partial class RegisterUserFormComponent
    {
        [Parameter]
        public RegisterUserFormModel Value { get; set; }

        public void SetSexToMale()
        {
            Value.Sex = 'M';
        }

        public void SetSexToFemale()
        {
            Value.Sex = 'F';
        }

        public void SetSexToOther()
        {
            Value.Sex = 'O';
        }

    }
}
