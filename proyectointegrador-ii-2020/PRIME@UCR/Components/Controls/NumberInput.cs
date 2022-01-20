using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace PRIME_UCR.Components.Controls
{
    public class NumberInput : GenericInput<double?>
    {
        protected override async Task OnChangeEvent(ChangeEventArgs e)
        {
            double result;
            var valid = Double.TryParse((string) e.Value, out result);
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