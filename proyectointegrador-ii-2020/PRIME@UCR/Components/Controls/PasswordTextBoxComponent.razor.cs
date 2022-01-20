using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Controls
{
    public partial class PasswordTextBoxComponent<TValue>
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool Required { get; set; } = true;
        //[Parameter] public string Type { get; set; } = "text";
        [Parameter] public string Type { get; set; } = "password";
        [Parameter] public string Id { get; set; } = null;


        //public string TxtType = "password";

        public void ShowPassword()
        {
            if (this.Type == "password")
            {
                this.Type = "text";
            }
            else
            {
                this.Type = "password";
            }
        }

        public string ValidationCssClass => ValidationUtils.ToBootstrapValidationCss(CssClass);

        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            result = Value;
            validationErrorMessage = "";
            return true;
        }

        protected async Task OnChangeEvent(ChangeEventArgs e)
        {
            Value = (TValue)e.Value;
            await ValueChanged.InvokeAsync(Value);
            EditContext.NotifyFieldChanged(FieldIdentifier);
        }
    }
}
