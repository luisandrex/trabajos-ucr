using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.MedicalRecords
{
    public partial class Filter
    {
        [Parameter] public EventCallback<string> SetFilter { get; set; }

        [Parameter] public EventCallback ClearFilter { get; set; }
        [Parameter] public string FilterBox { get; set; } = string.Empty;


        protected override void OnParametersSet()
        {

            base.OnParametersSet();
        }

        private async Task set_filter(string filter_box)
        {

            await SetFilter.InvokeAsync(filter_box);
        }


    }
}
