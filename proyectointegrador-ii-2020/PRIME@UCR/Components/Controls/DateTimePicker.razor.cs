using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;

namespace PRIME_UCR.Components.Controls
{
    public partial class DateTimePicker
    {
        private TimeSpan? _time; 
        [Parameter] public string DateLabel { get; set; }
        [Parameter] public string TimeLabel { get; set; }
        [Parameter] public DateTime? Min { get; set; }
        [Parameter] public DateTime? Max { get; set; }
        private string ValidationCssClass => ValidationUtils.ToBootstrapValidationCss(CssClass);

        async Task OnDateChanged(ChangeEventArgs e)
        {
            var valid = DateTime.TryParse((string)e.Value, out var result);
            if (valid)
            {
                
                Value = result + _time;
            }
            else
            {
                Value = null;
            }

            await ValueChanged.InvokeAsync(Value);
            EditContext.NotifyFieldChanged(FieldIdentifier);
        }

        async Task OnTimeChanged(ChangeEventArgs e)
        {
            var valid = TimeSpan.TryParse((string) e.Value, out var result);
            if (valid)
            {
                _time = result;
                Value = Value?.Date + _time;
            }
            else
            {
                _time = null;
            }

            await ValueChanged.InvokeAsync(Value);
            EditContext.NotifyFieldChanged(FieldIdentifier);
        }

        protected override void OnInitialized()
        {
            if (Value != null)
                _time = ((DateTime)Value).TimeOfDay;
        }

        protected override bool TryParseValueFromString(string value, out DateTime? result, out string validationErrorMessage)
        {
            result = Value;
            validationErrorMessage = null;
            return true;
        }
    }
}