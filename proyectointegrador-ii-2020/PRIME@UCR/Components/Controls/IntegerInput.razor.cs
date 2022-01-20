using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace PRIME_UCR.Components.Controls
{
    public partial class IntegerInput
    {
        [Parameter] public int? Min { get; set; }
        [Parameter] public int? Max { get; set; }
        [Parameter] public int? Step { get; set; }

        protected override async Task OnChangeEvent(ChangeEventArgs e)
        {
            int result;
            var valid = Int32.TryParse((string) e.Value, out result);
            if (valid)
            {
                Value = result;
            }
            else
            {
                Value = null;
            }

            await ValueChanged.InvokeAsync(Value);
            EditContext.NotifyFieldChanged(FieldIdentifier);
        }
        
        protected override void OnInitialized()
        {
            Type = "number";
        }
    }
}