using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace PRIME_UCR.Components.Controls
{
    public partial class DatePicker
    {
        [Parameter] public DateTime? Min { get; set; } 
        [Parameter] public DateTime? Max { get; set; } 
        
        protected override async Task OnChangeEvent(ChangeEventArgs e)
        {
            var valid = DateTime.TryParse((string) e.Value, out var result);
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
            Type = "date";
        }
    }
}