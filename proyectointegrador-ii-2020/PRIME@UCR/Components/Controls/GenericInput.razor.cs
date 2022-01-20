using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace PRIME_UCR.Components.Controls
{
    public partial class GenericInput<TValue>
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool Required { get; set; } = true;
        [Parameter] public string Type { get; set; } = "text";
        [Parameter] public string Id { get; set; } = null;

        public string ValidationCssClass => ValidationUtils.ToBootstrapValidationCss(CssClass);

        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            result = Value;
            validationErrorMessage = "";
            return true;
        }

        protected virtual Task OnChangeEvent(ChangeEventArgs e)
        {
            throw new NotImplementedException("Must override OnChangeEvent");
        }
    }
}
