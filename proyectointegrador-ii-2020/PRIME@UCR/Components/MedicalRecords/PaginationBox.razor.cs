using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PRIME_UCR.Components.MedicalRecords
{
    public partial class PaginationBox
    {
        [Parameter] public EventCallback<int> SetRecordsPerPage { get; set; }
        [Parameter] public int RecordsPerPage { get; set; } = 0;


        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        private async Task set_records_per_page() {

            await SetRecordsPerPage.InvokeAsync(RecordsPerPage); 
        }
    }
}
