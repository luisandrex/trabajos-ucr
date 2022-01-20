using Microsoft.AspNetCore.Components;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Pages.Appointments
{
    public partial class CreateMedicalAppointment
    {
        [Parameter] public string ExpId { set; get; }
        private Expediente recordModel;


        protected override async Task OnInitializedAsync()
        {
            recordModel = await MedicalRecordService.GetMedicalRecordDetailsLinkedAsync(Convert.ToInt32(ExpId));
        }
    }
}
