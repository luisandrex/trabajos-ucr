using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PRIME_UCR.Application.Services.UserAdministration;
using PRIME_UCR.Domain.Models.UserAdministration;
using PRIME_UCR.Application.Services.MedicalRecords;
using PRIME_UCR.Application.DTOs.MedicalRecords;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Components.MedicalRecords.Tabs
{
    public partial class GeneralInfoTab
    {
        [Parameter] public RecordViewModel record { get; set; }
        [Parameter] public Cita LastAppointment { get; set; }
        //[Inject] public IMedicalRecordService _service { get; set; }

        //private String _register;

        protected override async Task OnInitializedAsync()
        {
            //Persona person = await PersonService.GetPersonByIdAsync(DetailsModel.AdminId);
            //_register = person.Nombre + " " + person.PrimerApellido + " " + person.SegundoApellido;
        }
    }
}
